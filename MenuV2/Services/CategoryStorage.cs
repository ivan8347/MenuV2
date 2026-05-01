using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuV2.Services
{
    public static class CategoryStorage
    {
        private static readonly string FilePath = "categories.txt";

        public static List<string> Categories { get; private set; } = new List<string>();

        public static void Load()
        {
            if (File.Exists(FilePath))
                Categories = File.ReadAllLines(FilePath).ToList();
            else
                Categories = new List<string>();
        }

        public static void Save()
        {
            File.WriteAllLines(FilePath, Categories);
        }

        public static void Add(string category)
        {
            if (!Categories.Contains(category))
            {
                Categories.Add(category);
                Save();
            }
        }
    }

}
