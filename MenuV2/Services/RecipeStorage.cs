using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MenuV2.Core;

namespace MenuV2.Core
{
    public static class RecipeStorage
    {
        public static List<Recipe> Recipes { get; set; }

      

        static RecipeStorage() { Recipes = RecipeSerializer.Load(); }
        public static void Add(Recipe recipe)
        {
            Recipes.Add(recipe);
            Save();
        }

        public static void Remove(Recipe recipe) { Recipes.Remove(recipe); Save(); }
        public static Recipe Find(string name)
        {
            return Recipes
                    .FirstOrDefault(r => r.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
        public static void Reload()
        {
            Recipes = RecipeSerializer.Load();
        }
        public static void Save() 
        {
            RecipeSerializer.Save(Recipes);
        }

    }
}
