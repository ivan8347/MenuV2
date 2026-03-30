using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuV2.Core
{
    public class Product
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }   
        public double Fat { get; set; }       
        public double Carbs { get; set; }     
        public double BreadUnits { get; set; }
        public double PricePerKg { get; set; }
        public string Store { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
