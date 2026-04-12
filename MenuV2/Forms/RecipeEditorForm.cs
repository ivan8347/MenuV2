using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Services;

namespace MenuV2.Forms
{
    public partial class RecipeEditorForm : Form
    {
        public Recipe Recipe { get; private set; }
        private string _photoPath;

        // Конструктор для нового рецепта
        public RecipeEditorForm()
        {
            InitializeComponent();
            InitWindow();

            Recipe = new Recipe();
            LoadRecipe();
        }

        // Конструктор для редактирования существующего рецепта
        public RecipeEditorForm(Recipe recipe)
        {
            InitializeComponent();
            InitWindow();

            Recipe = recipe;
            LoadRecipe();
        }

        private void InitWindow()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

            this.ClientSize = new Size(
                (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8),
                (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8)
            );

            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
        }

        private void LoadRecipe()
        {
            txtName.Text = Recipe.Name;
            txtInstructions.Text = Recipe.Instructions;
            txtVideo.Text = Recipe.VideoUrl;
            txtCategory.Text = Recipe.Category;

            txtIngredients.Text = "";
            foreach (var ing in Recipe.Ingredients)
                txtIngredients.AppendText($"• {ing.OriginalText} ({ing.Weight} г){Environment.NewLine}");

            if (!string.IsNullOrEmpty(Recipe.PhotoPath) && File.Exists(Recipe.PhotoPath))
            {
                using (var img = Image.FromFile(Recipe.PhotoPath))
                {
                    picPhoto.Image = new Bitmap(img);
                }
                _photoPath = Recipe.PhotoPath;
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

                    _photoPath = Path.GetFullPath(destPath);

                    using (var img = Image.FromFile(_photoPath))
                    {
                        picPhoto.Image = new Bitmap(img);
                    }
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

            var service = new YouTubeService("AIzaSyCYQPqDOFD99Aven7RknPBtXFrOZm95Yfc");
            var recipe = await service.LoadRecipeFromYoutube(url);

            if (recipe == null)
            {
                MessageBox.Show("Не удалось загрузить данные YouTube.");
                return;
            }

            // Обновляем форму (БЕЗ автосохранения)
            Recipe.Name = recipe.Name;
            Recipe.Instructions = recipe.Instructions;
            Recipe.VideoUrl = recipe.VideoUrl;
            Recipe.PhotoPath = recipe.PhotoPath;
            Recipe.Category = recipe.Category;

            Recipe.Ingredients.Clear();
            foreach (var ing in recipe.Ingredients)
                Recipe.Ingredients.Add(ing);

            LoadRecipe();
        }

        private void btnParseIngredients_Click(object sender, EventArgs e)
        {
            var items = IngredientParser.FromText(txtInstructions.Text);

            // 1. Заполняем ингредиенты
            txtIngredients.Text = "";
            foreach (var ing in items)
            {
                txtIngredients.AppendText(
                    $"• {ing.OriginalText} ({ing.Weight} г){Environment.NewLine}"
                );
            }

            // 2. Удаляем ингредиенты из инструкции
            var lines = txtInstructions.Text
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var cleaned = new List<string>();

            foreach (var line in lines)
            {
                string l = line.Trim().ToLower();
                bool isIngredient = false;

                // 1. Если строка совпадает с найденным ингредиентом
                foreach (var ing in items)
                {
                    if (l.Contains(ing.OriginalText.ToLower()))
                    {
                        isIngredient = true;
                        break;
                    }
                }

                // 2. Если строка содержит число И единицу измерения
                bool hasDigit = Regex.IsMatch(l, @"\d");
                bool hasUnit = Regex.IsMatch(l, @"\b(г|гр|грамм|кг|мл|л|ст\.л|ч\.л|шт|стакан)\b");

                if (hasDigit && hasUnit)
                    isIngredient = true;

                // 3. Если строка содержит словесную меру (по вкусу, щепотка…)
                foreach (var kv in IngredientParser.WordAmountsKeys)
                {
                    if (l.Contains(kv))
                    {
                        isIngredient = true;
                        break;
                    }
                }

                if (!isIngredient)
                    cleaned.Add(line);

            }

            txtInstructions.Text = string.Join(Environment.NewLine, cleaned);
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Сохраняем только по кнопке
            Recipe.Name = txtName.Text;
            Recipe.Instructions = txtInstructions.Text;
            Recipe.VideoUrl = txtVideo.Text;
            Recipe.PhotoPath = _photoPath;
            Recipe.Category = txtCategory.Text;

            Recipe.Ingredients.Clear();
            var parsed = IngredientParser.FromText(txtIngredients.Text);
            foreach (var ing in parsed)
                Recipe.Ingredients.Add(ing);

            RecipeStorage.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
