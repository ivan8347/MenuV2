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
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(CategoryStorage.Categories.ToArray());

            if (!string.IsNullOrEmpty(_recipe.Category))
                cmbCategory.SelectedItem = _recipe.Category;



            // Ингредиенты → в текстовое поле
            txtIngredients.Text = "";
            foreach (var ing in _recipe.Ingredients)
            {
                // Вариант 1 — оригинальный текст + граммы

                txtIngredients.AppendText
               (
                 $"{ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
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

                txtIngredients.AppendText
                (
                  $"{ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                 );
        }

      
        private void btnParseIngredients_Click(object sender, EventArgs e)
        {
            var items = IngredientParser.FromText(txtInstructions.Text);

            txtIngredients.Text = "";
            foreach (var ing in items)
                txtIngredients.AppendText
               (
                 $"{ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                );
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            _recipe.Name = txtName.Text;
            _recipe.Instructions = txtInstructions.Text;
            _recipe.VideoUrl = txtVideo.Text;
            _recipe.PhotoPath = _photoPath;
            _recipe.Category = cmbCategory.SelectedItem?.ToString() ?? "";


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



        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            string html = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
            html = System.Text.Json.JsonSerializer.Deserialize<string>(html);

            var data = InstagramParser.Parse(html);
            if (data == null)
                return;

            // Название
            txtName.Text = data.Title;

            // Фото
            if (data.PhotoUrl != null)
            {
                var http = new HttpClient();
                var stream = await http.GetStreamAsync(data.PhotoUrl);
                picPhoto.Image = Image.FromStream(stream);
            }

            // Ингредиенты
           var ingredients = IngredientParser.FromText(data.IngredientsText);
            txtIngredients.Text = "";
            foreach (var ing in ingredients)
                txtIngredients.AppendText($"{ing.OriginalText}\n");

            // Инструкция
            txtInstructions.Text = data.Instructions;
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = ShowInput("Новая категория", "Введите название категории:");


            if (string.IsNullOrWhiteSpace(name))
                return;

            name = name.Trim();

            CategoryStorage.Add(name);

            cmbCategory.Items.Add(name);
            cmbCategory.SelectedItem = name;
        }

        public static string ShowInput(string title, string prompt)
        {
            Form form = new Form();
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Width = 450;
            form.Height = 220; // ← увеличил высоту

            Label lbl = new Label();
            lbl.Text = prompt;
            lbl.Font = new Font("Segoe UI", 14);
            lbl.AutoSize = true;
            lbl.Left = 20;
            lbl.Top = 20;

            TextBox box = new TextBox();
            box.Font = new Font("Segoe UI", 14);
            box.Left = 20;
            box.Top = 70;
            box.Width = 390;

            Button ok = new Button();
            ok.Text = "OK";
            ok.Font = new Font("Segoe UI", 12);
            ok.Width = 100;
            ok.Height = 40;
            ok.Left = form.ClientSize.Width - ok.Width - 20; // справа
            ok.Top = form.ClientSize.Height - ok.Height - 20; // снизу
            ok.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ok.DialogResult = DialogResult.OK;

            Button cancel = new Button();
            cancel.Text = "Отмена";
            cancel.Font = new Font("Segoe UI", 12);
            cancel.Width = 100;
            cancel.Height = 40;
            cancel.Left = ok.Left - cancel.Width - 10; // слева от OK
            cancel.Top = ok.Top;
            cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancel.DialogResult = DialogResult.Cancel;

            form.Controls.Add(lbl);
            form.Controls.Add(box);
            form.Controls.Add(ok);
            form.Controls.Add(cancel);

            form.AcceptButton = ok;
            form.CancelButton = cancel;

            return form.ShowDialog() == DialogResult.OK ? box.Text : null;
        }


    }
}
