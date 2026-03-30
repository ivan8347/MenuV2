using System;
using System.Collections.Generic;
using System.IO;
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
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()

        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProductStorage.Load();
            AllocConsole();



            /*var service = new OnlineProductInfoService();
            var p = service.GetProductInfo("картошка");


            if (p == null)
            {
                MessageBox.Show("p == null (продукт не найден)");
                return;
            }
           var ing = new Ingredient("картошка", 50, p);


            Console.WriteLine($"Ингредиент: {ing.Name}, {ing.Weight} г");
            Console.WriteLine($"Калории: {ing.Calories}");
            Console.WriteLine($"Белки: {ing.Protein}");
            Console.WriteLine($"Жиры: {ing.Fat}");
            Console.WriteLine($"Углеводы: {ing.Carbs}");

            MessageBox.Show(
                $"Калории: {p.Calories}\n" +
                $"Белки: {p.Protein}\n" +
                $"Жиры: {p.Fat}\n" +
                $"Углеводы: {p.Carbs}\n" +
                $"ХЕ: {p.BreadUnits}\n" +
                $"Источник: {p.Store}\n" +
                $"Обновлено: {p.UpdatedAt}"
            );*/

            /* var service = new OnlineProductInfoService();

             var p1 = service.GetProductInfo("картошка");
             var p2 = service.GetProductInfo("масло");

             var ing1 = new Ingredient("картошка", 150, p1);
             var ing2 = new Ingredient("масло", 10, p2);

             var recipe = new Recipe("Жареная картошка");
             recipe.AddIngredient(ing1);
             recipe.AddIngredient(ing2);

             MessageBox.Show(
                 $"Блюдо: {recipe.Name}\n" +
                 $"Вес блюда: {recipe.TotalWeight} г\n\n" +
                 $"Калории всего: {recipe.TotalCalories}\n" +
                 $"Калории на 100 г: {recipe.CaloriesPer100g}\n\n" +
                 $"Белки на 100 г: {recipe.ProteinPer100g}\n" +
                 $"Жиры на 100 г: {recipe.FatPer100g}\n" +
                 $"Углеводы на 100 г: {recipe.CarbsPer100g}\n" +
                 $"ХЕ на 100 г: {recipe.BreadUnitsPer100g}"
             );*/

            var service = new OnlineProductInfoService();
            var p = service.GetProductInfo("яйцо");


            var recipes = RecipeStorage.Recipes;

            

            var r = new Recipe("Омлет");
            r.AddIngredient(new Ingredient("яйцо", 100, p));

            RecipeStorage.Add(r);

            Console.WriteLine("Рецепт добавлен!");




            /*var recipes = RecipeSerializer.Load();

            var service = new OnlineProductInfoService();
            var p = service.GetProductInfo("картошка");
            var ing = new Ingredient("картошка", 50, p);

            var recipe = new Recipe("Картофель отварной");
            recipe.AddIngredient(ing);

            recipes.Add(recipe);

            RecipeSerializer.Save(recipes);

            Console.WriteLine("Рецепт сохранён!");*/


            Console.WriteLine("Рабочая директория: " + Environment.CurrentDirectory);
            Console.WriteLine("Путь к рецептам: " + Path.GetFullPath("DATA/recipes.json"));

            RecipeStorage.Reload();

            Console.WriteLine($"Загружено рецептов: {recipes.Count}");
            Console.WriteLine("=== Проверка загрузки рецептов ===");

            foreach (var recipe in RecipeStorage.Recipes)
            {
                Console.WriteLine($"Рецепт: {recipe.Name}");
                Console.WriteLine($"  Ингредиентов: {recipe.Ingredients.Count}");

                foreach (var ing in recipe.Ingredients)
                {
                    Console.WriteLine($"    - {ing.Name}, {ing.Weight} г, Ккал: {ing.Calories}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("=== Конец списка ===");


            Console.WriteLine("Файл рецептов: " + Path.GetFullPath("DATA/recipes.json"));
            Console.WriteLine("Содержимое файла:");
            Console.WriteLine(File.ReadAllText("DATA/recipes.json"));



            Application.Run(new RecipeListForm());

        }
    }




   


}
