using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MenuV2.Core;
using System.Windows.Forms;

namespace MenuV2.Services
{
    public static class ProductStorage
    {
        private static readonly string FilePath = Path.Combine("Data", "products.json");

        private static List<Product> _products = new List<Product>();

        public static IReadOnlyList<Product> Products => _products;

        // Загружаем JSON при старте приложения
        public static void Load()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            if (!File.Exists(FilePath))
            {
                _products = new List<Product>();
                Save();
                return;
            }

            var json = File.ReadAllText(FilePath);
            _products = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        // Сохраняем JSON
        public static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            var json = JsonConvert.SerializeObject(_products, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        // Добавление или обновление продукта
        public static void AddOrUpdate(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                _products.Remove(existing);
            }

            _products.Add(product);
            Save();
        }

        // Поиск по точному имени
        public static Product Find(string name)
        {
            return _products.FirstOrDefault(p =>
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Fuzzy-поиск (мука → мука пшеничная)
        public static Product FindSimilar(string name)
        {
            return _products
                .OrderBy(p => LevenshteinDistance(p.Name.ToLower(), name.ToLower()))
                .FirstOrDefault();
        }

        // Проверка свежести данных (например, 30 дней)
        public static bool IsFresh(Product product)
        {
            return (DateTime.Now - product.UpdatedAt).TotalDays < 30;
        }

        // Алгоритм Левенштейна для fuzzy-поиска
        private static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,
                            d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
    }
}
