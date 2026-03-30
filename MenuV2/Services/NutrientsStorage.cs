using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MenuV2.Services
{
    public static class NutrientsStorage
    {
        private static Dictionary<string, NutrientInfo> _data;
        private static readonly object _lock = new object();

            // Гарантированно правильный путь к Data/nutrients.json
            private const string FilePath = "DATA/nutrients.json";

     

        public static void Load()
        {

            lock (_lock)
            {
                if (_data != null)
                    return;

                string path = FilePath;


                try
                {
                    if (!File.Exists(path))
                    {
                        Console.WriteLine($"[NutrientsStorage] Файл не найден: {path}");
                        _data = new Dictionary<string, NutrientInfo>(StringComparer.OrdinalIgnoreCase);
                        return;
                    }

                    string json = File.ReadAllText(path);

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        Console.WriteLine("[NutrientsStorage] Файл пустой.");
                        _data = new Dictionary<string, NutrientInfo>(StringComparer.OrdinalIgnoreCase);
                        return;
                    }

                    // Попытка десериализации массива
                    var list = JsonConvert.DeserializeObject<List<NutrientInfo>>(json);

                    if (list == null)
                    {
                        Console.WriteLine("[NutrientsStorage] JSON не содержит массив.");
                        _data = new Dictionary<string, NutrientInfo>(StringComparer.OrdinalIgnoreCase);
                        return;
                    }

                    _data = new Dictionary<string, NutrientInfo>(StringComparer.OrdinalIgnoreCase);

                    foreach (var item in list)
                    {
                        if (item?.Name == null)
                            continue;

                        _data[item.Name.Trim().ToLower()] = item;
                    }

                    Console.WriteLine($"[NutrientsStorage] Загружено {_data.Count} продуктов.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[NutrientsStorage] Ошибка загрузки JSON:");
                    Console.WriteLine(ex);

                    _data = new Dictionary<string, NutrientInfo>(StringComparer.OrdinalIgnoreCase);
                }
            }
        }

        public static NutrientInfo Find(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            if (_data == null)
                Load();

            string key = name.Trim().ToLower();

            _data.TryGetValue(key, out var info);
            return info;
        }
    }
    

    public class NutrientInfo
    {
        public string Name { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }
        public double Calories { get; set; }
    }
}
