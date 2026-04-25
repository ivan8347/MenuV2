using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Services;
using Microsoft.Web.WebView2.Core;

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

            MessageBox.Show(Application.ExecutablePath);

            // Ингредиенты → в текстовое поле
            txtIngredients.Text = "";
            foreach (var ing in _recipe.Ingredients)
            {
                // Вариант 1 — оригинальный текст + граммы

                txtIngredients.AppendText
               (
                 $"• {ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                );
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
                webView.NavigationCompleted += WebView_NavigationCompleted;
                await webView.EnsureCoreWebView2Async();
                webView.Source = new Uri(url);
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
                txtIngredients.AppendText($"{ing.Name} — {ing.Weight} г\n");
        }

        private void btnParseIngredients_Click(object sender, EventArgs e)
        {
            var items = IngredientParser.FromText(txtInstructions.Text);

            txtIngredients.Text = ""; // очищаем

            txtIngredients.Text = "";
            foreach (var ing in items)
            {
                txtIngredients.AppendText(
                    $"• {ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                );
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            _recipe.Name = txtName.Text;
            _recipe.Instructions = txtInstructions.Text;
            _recipe.VideoUrl = txtVideo.Text;
            _recipe.PhotoPath = _photoPath;
            _recipe.Category = txtCategory.Text;

            // Перезаписываем ингредиенты
            _recipe.Ingredients.Clear();

            var parsed = IngredientParser.FromText(txtInstructions.Text);
            foreach (var ing in parsed)
                _recipe.Ingredients.Add(ing);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Получаем HTML из WebView2
            string html = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
            html = System.Text.Json.JsonSerializer.Deserialize<string>(html);

            // Сохраняем HTML для анализа
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "insta.html");
            File.WriteAllText(path, html);

            // Ищем фото (Reels)
            var imgMatch = Regex.Match(html,
                @"""image_versions2"":\{""candidates"":

\[\{""url"":""(https:[^""]+)""");

            if (!imgMatch.Success)
            {
                imgMatch = Regex.Match(html,
                    @"""fallback_url"":""(https:[^""]+)""");
            }

            // Ищем название
            var titleMatch = Regex.Match(html,
                @"""caption"":\{""text"":""(.*?)""");

            // Загружаем фото
            if (imgMatch.Success)
            {
                var http = new HttpClient();
                var stream = await http.GetStreamAsync(imgMatch.Groups[1].Value);
                picPhoto.Image = Image.FromStream(stream);
            }

            // Заполняем название
            if (titleMatch.Success)
            {
                string titleRaw = titleMatch.Groups[1].Value;

                // Декодируем \uXXXX
                string title = Regex.Unescape(titleRaw);

                // Декодируем HTML-сущности (&amp; → &, &quot; → ")
                title = System.Net.WebUtility.HtmlDecode(title);

                // Берём только первую строку (если нужно)
                title = title.Split('\n')[0];

                txtName.Text = title;
            }



            MessageBox.Show("Instagram загружен");
        }


    }
}
