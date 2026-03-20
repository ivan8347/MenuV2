using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MenuV2.Core;
using MenuV2.Forms;
using MenuV2.Services;


namespace MenuV2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProductStorage.Load();
            ProductStorage.AddOrUpdate(new Product
            {
                Name = "мука пшеничная",
                Calories = 364,
                BreadUnits = 2.0,
                PricePerKg = 52,
                Store = "Пятёрочка",
                UpdatedAt = DateTime.Now
            });


            Application.Run(new RecipeListForm());

        }
    }
}
