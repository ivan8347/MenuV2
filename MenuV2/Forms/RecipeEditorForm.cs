using System;
using System.Windows.Forms;
using MenuV2.Services;
using MenuV2.Models;

namespace MenuV2
{
    public partial class RecipeEditorForm : Form
    {
        private readonly UniversalVideoLoader _loader;

        public RecipeEditorForm()
        {
            InitializeComponent();

            // ВСТАВЬ СВОЙ РАБОЧИЙ VK ТОКЕН
            _loader = new UniversalVideoLoader("vk1.a.ТВОЙ_ТОКЕН");
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            string url = txtVideoUrl.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Введите ссылку на видео");
                return;
            }

            try
            {
                btnLoad.Enabled = false;
                btnLoad.Text = "Загрузка...";

                VideoInfo info = await _loader.LoadAsync(url);

                // Заполняем поля
                txtTitle.Text = info.Title;
                txtDescription.Text = info.Description;
               // txtImageUrl.Text = info.ImageUrl;

                // Загружаем превью
                if (!string.IsNullOrEmpty(info.ImageUrl))
                    pictureBoxPreview.Load(info.ImageUrl);

                // Сохраняем прямую ссылку на видео
                //txtVideoFile.Text = info.VideoUrl;

                MessageBox.Show($"Видео загружено ({info.Platform})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                btnLoad.Enabled = true;
                btnLoad.Text = "Загрузить";
            }
        }
    }
}
