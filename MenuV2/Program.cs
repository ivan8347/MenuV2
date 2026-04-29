using System;
using System.Text;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Forms;
using MenuV2.Services;

namespace MenuV2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Если нужно – подгружаем данные перед запуском UI
            NutrientsImporter.ImportAndUpdateProducts();


            ProductStorage.Load();

            Application.Run(new RecipeListForm());
        }
    }
}
