using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MenuV2.Core
{
    public static class NutrientsImporter
    {
        private static readonly string NutrientsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA", "nutrients.json");

        private static readonly string ProductsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA", "products.json");

        public static void ImportIfProductsEmpty()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProductsPath));

            // Если products.json уже заполнен — ничего не делаем
            if (File.Exists(ProductsPath))
            {
                var existing = File.ReadAllText(ProductsPath);
                if (!string.IsNullOrWhiteSpace(existing) && existing.Trim() != "[]")
                    return;
            }

            // Если nutrients.json нет — импорт невозможен
            if (!File.Exists(NutrientsPath))
                return;

            var nutrientsJson = File.ReadAllText(NutrientsPath);
            var nutrients = JsonConvert.DeserializeObject<List<NutrientItem>>(nutrientsJson);

            if (nutrients == null || nutrients.Count == 0)
                return;

            var products = new List<Product>();

            foreach (var n in nutrients)
            {
                products.Add(new Product
                {
                    Name = n.Name,
                    Protein = n.Protein,
                    Fat = n.Fat,
                    Carbs = n.Carbs,
                    Calories = n.Calories,
                    BreadUnits = Math.Round(n.Carbs / 12.0, 2),
                    UpdatedAt = DateTime.Now
                });
            }

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(ProductsPath, json);
        }

        private class NutrientItem
        {
            public string Name { get; set; }
            public double Protein { get; set; }
            public double Fat { get; set; }
            public double Carbs { get; set; }
            public double Calories { get; set; }
        }
    }
}
