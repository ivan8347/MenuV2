using System;
using System.IO;
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
            this.Load += RecipeListForm_Load;
        }
        private void RecipeListForm_Load(object sender, EventArgs e)
        {
            ProductStorage.Load();
            RecipeStorage.Reload();             // ← читаем JSON
            LoadRecipes();                      // ← обновляем список
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
                    LoadRecipes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рецепта: " + ex.Message);
            }
        }
        private async void LoadVkRecipe(string url)
        {
            try
            {
                if (url.Contains("vkvideo.ru"))
                    url = url.Replace("vkvideo.ru", "vk.com");

                var vk = new VkApiVideoService("access_token=vk1.a.IbzY3cptgVq2keKBPNrAb1znAQlsv4Ms5dzO5lxfAYzOu30KrCVNXC2X356Mo9ezJbjlIXQcHxIesHhXYF4CteMNJifxvajl58gwuP2buxD4fMwPGhRjmVZkvBVM7ppDvowexfQQLrjQ6h-fzrS9IpvySxuakjXFBlrX-INR5xCtRRBi69767jQv2zf_7BEFHrCG1SJJuxnJAxDm2SwTWw&expires_in=86400&user_id=1111626609");
                var info = await vk.GetVideoAsync(url);

                var recipe = new Recipe
                {
                    Name = info.Title,
                    Instructions = info.Description,
                    VideoUrl = info.VideoUrl,
                    PhotoPath = SavePreview(info.PreviewUrl),
                    Category = ""
                };

                recipe.Ingredients = IngredientParser.FromText(info.Description);

                var editor = new RecipeEditorForm(recipe);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    // обновление списка, если нужно
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки VK API: " + ex.Message);
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
            if(text.Contains("vk.com"))
            {
                LoadVkRecipe(text); 
                return;
            }
            // Иначе создаём пустой рецепт
            var recipe = new Recipe();
            var editor = new RecipeEditorForm(recipe);

            if (editor.ShowDialog() == DialogResult.OK)
            {
                RecipeStorage.Add(recipe);
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

        private string SavePreview(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            Directory.CreateDirectory("photos");

            string fileName = "vk_" + Guid.NewGuid().ToString("N") + ".jpg";
            string path = Path.Combine("photos", fileName);

            using (var client = new System.Net.WebClient())
                client.DownloadFile(url, path);

            return path;
        }


    }
}
