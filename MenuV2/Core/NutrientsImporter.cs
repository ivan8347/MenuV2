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

        public static void ImportAndUpdateProducts()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProductsPath));

            // Загружаем nutrients.json
            if (!File.Exists(NutrientsPath))
                return;

            var nutrientsJson = File.ReadAllText(NutrientsPath);
            var nutrients = JsonConvert.DeserializeObject<List<NutrientItem>>(nutrientsJson);

            if (nutrients == null || nutrients.Count == 0)
                return;

            // Загружаем текущие продукты (если есть)
            List<Product> products = new List<Product>();

            if (File.Exists(ProductsPath))
            {
                var existingJson = File.ReadAllText(ProductsPath);
                products = JsonConvert.DeserializeObject<List<Product>>(existingJson)
                           ?? new List<Product>();
            }

            // Обновляем или добавляем продукты
            foreach (var n in nutrients)
            {
                var existing = products.Find(p =>
                    p.Name.Equals(n.Name, StringComparison.OrdinalIgnoreCase));

                if (existing == null)
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
                else
                {
                    existing.Protein = n.Protein;
                    existing.Fat = n.Fat;
                    existing.Carbs = n.Carbs;
                    existing.Calories = n.Calories;
                    existing.BreadUnits = Math.Round(n.Carbs / 12.0, 2);
                    existing.UpdatedAt = DateTime.Now;
                }
            }

            // Сохраняем обновлённый products.json
            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(ProductsPath, json);
        }

        // ВАЖНО: NutrientItem читает lowerCase ключи из nutrients.json
        private class NutrientItem
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("protein")]
            public double Protein { get; set; }

            [JsonProperty("fat")]
            public double Fat { get; set; }

            [JsonProperty("carbs")]
            public double Carbs { get; set; }

            [JsonProperty("calories")]
            public double Calories { get; set; }
        }
    }
}
