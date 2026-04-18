using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MenuV2.Core
{
    public class Product
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("calories")]
        public double Calories { get; set; }

        [JsonProperty("protein")]
        public double Protein { get; set; }

        [JsonProperty("fat")]
        public double Fat { get; set; }

        [JsonProperty("carbs")]
        public double Carbs { get; set; }

        public double BreadUnits { get; set; }
        public double PricePerKg { get; set; }
        public string Store { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
