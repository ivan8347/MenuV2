using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MenuV2.Core
{
    public static class RecipeSerializer
    {
        private static readonly string FilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA", "recipes.json");

        public static List<Recipe> Load()

        {

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            if (!File.Exists(FilePath))
                return new List<Recipe>();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Recipe>>(json)
                   ?? new List<Recipe>();
        }

        public static void Save(List<Recipe> recipes)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            var json = JsonConvert.SerializeObject(recipes, Formatting.Indented);
            File.WriteAllText(FilePath, json);

            Console.WriteLine("Сохранено в файл: " + Path.GetFullPath(FilePath));
        }
    }


}
