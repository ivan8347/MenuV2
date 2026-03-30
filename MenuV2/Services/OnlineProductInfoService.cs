using System;
using System.Threading.Tasks;
using MenuV2.Core;

namespace MenuV2.Services
{
    public class OnlineProductInfoService
    {
        private static string NormalizeName(string name)
        {
            return name?.Trim().ToLower();
        }

        public Product GetProductInfo(string name)
        {
            var normalized = NormalizeName(name);

            var info = NutrientsStorage.Find(normalized);
            if (info == null)
                return null;

            return new Product
            {
                Name = normalized,
                Protein = info.Protein,
                Fat = info.Fat,
                Carbs = info.Carbs,
                Calories = info.Calories,
                BreadUnits = info.Carbs / 12.0,
                //ручное заполнение
                Store = "NutrientsStorage",
                PricePerKg = 0,
                UpdatedAt = DateTime.Now
            };
        }
    }


}
