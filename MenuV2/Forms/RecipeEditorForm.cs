using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Services;
using Microsoft.Web.WebView2.Core;
using System.Linq;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Text;


namespace MenuV2.Forms
{
    public partial class RecipeEditorForm : Form
    {
        private Recipe _recipe;
        private string _photoPath;

        public RecipeEditorForm(Recipe recipe)
        {
            InitializeComponent();
            _recipe = recipe;
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            txtName.Text = _recipe.Name;
            txtInstructions.Text = _recipe.Instructions;
            txtVideo.Text = _recipe.VideoUrl;
            txtCategory.Text = _recipe.Category;


            // Ингредиенты → в текстовое поле
            txtIngredients.Text = "";
            foreach (var ing in _recipe.Ingredients)
            {
                // Вариант 1 — оригинальный текст + граммы

                // txtIngredients.AppendText
                //(
                //  $"• {ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                // );
                txtIngredients.AppendText($"{ing.OriginalText}{Environment.NewLine}");

            }



            if (!string.IsNullOrEmpty(_recipe.PhotoPath) && File.Exists(_recipe.PhotoPath))
            {
                picPhoto.Image = Image.FromFile(_recipe.PhotoPath);
                _photoPath = _recipe.PhotoPath;
            }
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.jpg;*.png;*.jpeg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory("photos");

                    string fileName = Path.GetFileName(dlg.FileName);
                    string destPath = Path.Combine("photos", fileName);

                    File.Copy(dlg.FileName, destPath, true);

                    _photoPath = destPath;
                    picPhoto.Image = Image.FromFile(destPath);
                }
            }
        }

        private async void btnLoadYoutube_Click(object sender, EventArgs e)
        {
            string url = txtVideo.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Введите ссылку на YouTube.");
                return;
            }

            if (url.Contains("instagram.com"))
            {
                string caption = await LoadInstagramCaptionAsync(url);

                if (caption == null)
                {
                    MessageBox.Show("Не удалось получить данные из Instagram.");
                    return;
                }

                // Название
                txtName.Text = caption.Split('\n')[0].Trim();

                // Инструкция
                txtInstructions.Text = caption;

                // Ингредиенты
                var ingredients = IngredientParser.FromText(caption);
                txtIngredients.Text = "";
                foreach (var ing in ingredients)
                    txtIngredients.AppendText(ing.OriginalText + Environment.NewLine);

                return;
            }




            var service = new YouTubeService("AIzaSyCYQPqDOFD99Aven7RknPBtXFrOZm95Yfc");
            var recipe = await service.LoadRecipeFromYoutube(url);

            if (recipe == null)
            {
                MessageBox.Show("Не удалось загрузить данные YouTube.");
                return;
            }

            txtName.Text = recipe.Name;
            txtInstructions.Text = recipe.Instructions;
            txtVideo.Text = recipe.VideoUrl;

            if (recipe.PhotoPath != null)
            {
                picPhoto.Image = Image.FromFile(recipe.PhotoPath);
                _photoPath = recipe.PhotoPath;
            }

            txtIngredients.Text = "";
            foreach (var ing in recipe.Ingredients)
                //txtIngredients.AppendText($"{ing.OriginalText}\n");
                txtIngredients.AppendText($"{ing.OriginalText}{Environment.NewLine}");
        }

        private void btnParseIngredients_Click(object sender, EventArgs e)
        {
            var items = IngredientParser.FromText(txtInstructions.Text);

          //  txtIngredients.Text = ""; // очищаем

            txtIngredients.Text = "";
            foreach (var ing in items)
            {
                // txtIngredients.AppendText($"{ing.OriginalText}\n");
                txtIngredients.AppendText($"{ing.OriginalText}{Environment.NewLine}");
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            _recipe.Name = txtName.Text;
            _recipe.Instructions = txtInstructions.Text;
            _recipe.VideoUrl = txtVideo.Text;
            _recipe.PhotoPath = _photoPath;
            _recipe.Category = txtCategory.Text;

            // Сохраняем ингредиенты ТОЛЬКО из txtIngredients
            _recipe.Ingredients.Clear();

           var items = IngredientParser.FromText(txtIngredients.Text);
            foreach (var ing in items)
                _recipe.Ingredients.Add(ing);

            DialogResult = DialogResult.OK;
            Close();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        public static async Task<string> LoadInstagramCaptionAsync(string url)
        {
            // Извлекаем shortcode
            var m = Regex.Match(url, @"instagram\.com\/(?:reel|p)\/([^\/\?]+)");
            if (!m.Success)
                return null;

            string shortcode = m.Groups[1].Value;
            string embedUrl = "https://www.instagram.com/reel/" + shortcode + "/embed/captioned/";

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                // ВАЖНО: НЕ используем GetStringAsync
                var bytes = await http.GetByteArrayAsync(embedUrl);

                // Декодируем вручную, игнорируя заголовок Instagram
                string html = Encoding.UTF8.GetString(bytes);

                // Ищем caption
                var match = Regex.Match(html, @"<meta property=""og:description"" content=""([^""]+)""");
                if (match.Success)
                {
                    string caption = System.Net.WebUtility.HtmlDecode(match.Groups[1].Value);
                    return caption;
                }
            }

            return null;
        }




    }
}

