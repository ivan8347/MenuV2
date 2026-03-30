using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MenuV2.Core
{
    public static class RecipeSerializer
    {
        private static readonly string FilePath = "DATA/recipes.json";

        public static List<Recipe> Load()
        {
            if (!File.Exists(FilePath))
                return new List<Recipe>();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Recipe>>(json)
                   ?? new List<Recipe>();
        }

        public static void Save(List<Recipe> recipes)
        {
            var json = JsonConvert.SerializeObject(recipes, Formatting.Indented);
            File.WriteAllText(FilePath, json);

            Console.WriteLine("Сохранено в файл: " + Path.GetFullPath(FilePath));
        }
    }
}
