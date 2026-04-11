using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace MenuV2.Core
{
    public class Recipe
    {
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public string VideoUrl { get; set; }
        public string Instructions { get; set; }
        public string Category { get; set; }
        public List<string> Steps { get; set; } = new List<string>();


        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
          public double TotalCalories => Ingredients.Sum(i => i.Calories);
          public double TotalProtein => Ingredients.Sum(i => i.Protein);
          public double TotalFat => Ingredients.Sum(i => i.Fat);
          public double TotalCarbs => Ingredients.Sum(i =>i.Carbs);
          public double TotalBreadUnits => Ingredients.Sum(i => i.BreadUnits);
             //вес блюда
          public double TotalWeight => Ingredients.Sum(i => i.Weight);

           // БЖУ на 100 г блюда
          public double CaloriesPer100g => TotalCalories / TotalWeight * 100;
          public double ProteinPer100g => TotalProtein / TotalWeight * 100;
          public double FatPer100g => TotalFat / TotalWeight * 100;
          public double CarbsPer100g => TotalCarbs / TotalWeight * 100;
          public double BreadUnitsPer100g => TotalBreadUnits / TotalWeight * 100;

        public Recipe (string name)
        {
            Name = name;
        }
        public Recipe() { }   // ← добавить

        public void AddIngredient(Ingredient ing)
        {
            Ingredients.Add(ing);
        }


    }
}
