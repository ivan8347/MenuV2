using System;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Services;

namespace MenuV2.Forms
{
    public partial class RecipeListForm : Form
    {
        public RecipeListForm()
        {
            InitializeComponent();
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            listRecipes.DataSource = null;
            listRecipes.DataSource = RecipeStorage.Recipes;
            listRecipes.DisplayMember = "Name";
        }

        private async void LoadYouTubeRecipe(string url)
        {
            try
            {
                var service = new YouTubeService("ТВОЙ_API_KEY");
                var recipe = await service.LoadRecipeFromYoutube(url);

                if (recipe == null)
                {
                    MessageBox.Show("Не удалось загрузить рецепт.");
                    return;
                }

                var editor = new RecipeEditorForm(recipe);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    RecipeStorage.Save();
                    LoadRecipes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рецепта: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = Clipboard.ContainsText() ? Clipboard.GetText().Trim() : "";

            // Если в буфере есть YouTube‑ссылка → загружаем рецепт
            if (text.Contains("youtu.be") || text.Contains("youtube.com/watch"))
            {
                LoadYouTubeRecipe(text);
                return;
            }

            // Иначе создаём пустой рецепт
            var recipe = new Recipe("Новый рецепт");
            RecipeStorage.Add(recipe);

            var editor = new RecipeEditorForm(recipe);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                RecipeStorage.Save();
                LoadRecipes();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listRecipes.SelectedItem is Recipe recipe)
            {
                var form = new RecipeEditorForm(recipe);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RecipeStorage.Save();
                    LoadRecipes();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listRecipes.SelectedItem is Recipe recipe)
            {
                if (MessageBox.Show("Удалить рецепт?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RecipeStorage.Remove(recipe);
                    RecipeStorage.Save();
                    LoadRecipes();
                }
            }
        }

        private void listRecipes_DoubleClick(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }
    }
}
