using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Services;

namespace MenuV2.Forms
{
    public partial class RecipeListForm : Form
    {
        private Recipe selectedRecipe;
        private Panel selectedPanel;

        public RecipeListForm()
        {
            InitializeComponent();
            this.Load += RecipeListForm_Load;
        }

        private void RecipeListForm_Load(object sender, EventArgs e)
        {
            ProductStorage.Load();
            RecipeStorage.Reload();
            LoadRecipeCards();
            CategoryStorage.Load();

        }

        // ================================
        // ГАЛЕРЕЯ РЕЦЕПТОВ
        // ================================
        private void LoadRecipeCards()
        {
            selectedRecipe = null;
            selectedPanel = null;

            flowRecipes.Controls.Clear();

            foreach (var recipe in RecipeStorage.Recipes)
            {
                var card = CreateRecipeCard(recipe);
                flowRecipes.Controls.Add(card);
            }
        }

        private Control CreateRecipeCard(Recipe recipe)
        {
            var panel = new Panel();
            panel.Width = 200;
            panel.Height = 300;
            panel.Margin = new Padding(10);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Cursor = Cursors.Hand;
            panel.BackColor = Color.White;

            var pic = new PictureBox();
            pic.Width = 175;
            pic.Height = 180;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Left = 10;
            pic.Top = 10;

            if (!string.IsNullOrEmpty(recipe.PhotoPath) && File.Exists(recipe.PhotoPath))
                pic.Image = Image.FromFile(recipe.PhotoPath);
            else
                pic.Image = Properties.Resources.no_photo;

            var lbl = new Label();
            lbl.Text = recipe.Name;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(180, 0); // ширина фиксирована, высота растёт
            lbl.Left = 10;
            lbl.Top = 190;
            lbl.TextAlign = ContentAlignment.TopCenter;
            lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lbl.Cursor = Cursors.Hand;


            // === ОДИНАРНЫЙ КЛИК: выделение ===
            void selectHandler(object s, EventArgs e)
            {
                selectedRecipe = recipe;

                if (selectedPanel != null)
                    selectedPanel.BackColor = Color.White;

                panel.BackColor = Color.LightBlue;
                selectedPanel = panel;
            }

            // === ДВОЙНОЙ КЛИК: редактирование ===
            void openHandler(object s, EventArgs e)
            {
                OpenRecipe(recipe);
            }

            panel.Click += selectHandler;
            pic.Click += selectHandler;
            lbl.Click += selectHandler;

            panel.DoubleClick += openHandler;
            pic.DoubleClick += openHandler;
            lbl.DoubleClick += openHandler;

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);

            return panel;
        }




        private void OpenRecipe(Recipe recipe)
        {
            using (var f = new RecipeEditorForm(recipe))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    RecipeStorage.Save();
                    LoadRecipeCards();
                }
            }
        }

        // ================================
        // ДОБАВЛЕНИЕ РЕЦЕПТА
        // ================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            selectedRecipe = null;
            selectedPanel = null;

            string text = Clipboard.ContainsText() ? Clipboard.GetText().Trim() : "";

            // === YouTube ===
          /*  if (text.Contains("youtu.be") || text.Contains("youtube.com/watch"))
            {
                LoadYouTubeRecipe(text);
                return;
            }

            // === Instagram ===
            if (text.Contains("instagram.com") || text.Contains("igsh") || text.Contains("/reel/"))
            {
                string url = InstagramParser.NormalizeInstagramUrl(text);

                if (url != null)
                {
                    LoadInstagramRecipe(url);
                    return;
                }
            }*/

            // === Пустой рецепт ===
            var recipe = new Recipe();
            var editor = new RecipeEditorForm(recipe);

            if (editor.ShowDialog() == DialogResult.OK)
            {
                RecipeStorage.Add(recipe);
                RecipeStorage.Save();
                LoadRecipeCards();
            }
        }

        // ================================
        // ЗАГРУЗКА YOUTUBE
        // ================================
        private async void LoadYouTubeRecipe(string url)
        {
            try
            {
                var service = new YouTubeService("AIzaSyCYQPqDOFD99Aven7RknPBtXFrOZm95Yfc");
                var recipe = await service.LoadRecipeFromYoutube(url);

                if (recipe == null)
                {
                    MessageBox.Show("Не удалось загрузить рецепт.");
                    return;
                }

                var editor = new RecipeEditorForm(recipe);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    RecipeStorage.Add(recipe);
                    RecipeStorage.Save();
                    LoadRecipeCards();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рецепта: " + ex.Message);
            }
        }

        // ================================
        // ЗАГРУЗКА INSTAGRAM
        // ================================
        private async void LoadInstagramRecipe(string url)
        {
            try
            {
                var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");

                byte[] raw = await http.GetByteArrayAsync(url);
                string html = System.Text.Encoding.UTF8.GetString(raw);

                var data = InstagramParser.ParseFromHtml(html);
                if (data == null)
                {
                    MessageBox.Show("Не удалось загрузить данные Instagram.");
                    return;
                }

                var recipe = new Recipe
                {
                    Name = data.Title,
                    Instructions = data.Instructions,
                    Ingredients = IngredientParser.FromText(data.IngredientsText)
                };

                // Фото
                if (!string.IsNullOrEmpty(data.PhotoUrl))
                {
                    byte[] imgBytes = await http.GetByteArrayAsync(data.PhotoUrl);
                    Directory.CreateDirectory("photos");
                    string file = Path.Combine("photos", Guid.NewGuid() + ".jpg");
                    File.WriteAllBytes(file, imgBytes);
                    recipe.PhotoPath = file;
                }

                RecipeStorage.Add(recipe);
                RecipeStorage.Save();
                LoadRecipeCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки Instagram: " + ex.Message);
            }
        }

        // ================================
        // РЕДАКТИРОВАНИЕ / УДАЛЕНИЕ
        // ================================


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRecipe == null)
            {
                MessageBox.Show("Сначала выберите рецепт кликом по карточке.");
                return;
            }

            if (MessageBox.Show("Удалить рецепт?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RecipeStorage.Remove(selectedRecipe);
                RecipeStorage.Save();
                LoadRecipeCards();
                selectedRecipe = null;
            }
        }

      
    }
}
