using System;
using System.Windows.Forms;
using MenuV2.Forms;
using MenuV2.Services;

namespace MenuV2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Если нужно – подгружаем данные перед запуском UI
            ProductStorage.Load();

            Application.Run(new RecipeListForm());
        }
    }
}
