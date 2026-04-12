using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuV2.Core
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double Weight { get; set; }

        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }
        public double BreadUnits { get; set; }
        public string OriginalText { get; set; }
      



        public Ingredient(string name, double weight, Product product)
        {
            Name = name;
            Weight = weight;
            // Пересчёт БЖУ на вес ингредиента
            double factor = weight / 100;
            Calories = product.Calories * factor;
            Protein = product.Protein * factor;
            Fat = product.Fat * factor;
            Carbs = product.Carbs * factor;
            BreadUnits = product.BreadUnits * factor;


        }
        public Ingredient() { }   // ← добавить

        public void SetWeight(double newWeight, Product product)
        {
            Weight = newWeight;

            double factor = Weight / 100.0;

            Calories = product.Calories * factor;
            Protein = product.Protein * factor;
            Fat = product.Fat * factor;
            Carbs = product.Carbs * factor;
            BreadUnits = product.BreadUnits * factor;
        }

    }
}
