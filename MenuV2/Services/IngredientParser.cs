using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MenuV2.Core;

namespace MenuV2.Services
{
    public static class IngredientParser
    {
        public static IEnumerable<string> WordAmountsKeys => WordAmounts.Keys;


        // Словесные количества
        private static readonly Dictionary<string, double> WordAmounts =
            new Dictionary<string, double>
            {
                { "щепотка", 1 },
                { "щепотки", 1 },
                { "пучок", 30 },
                { "пучка", 30 },
                { "веточка", 3 },
                { "веточки", 3 },
                { "кусочек", 20 },
                { "кусочка", 20 },
                { "по вкусу", 1 }
            };

        public static List<Ingredient> FromText(string text)
        {
            var list = new List<Ingredient>();

            if (string.IsNullOrWhiteSpace(text))
                return list;

            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in lines)
            {
                string line = raw.Trim().ToLower();

                if (line.Length < 2)
                    continue;

              

                // 2. Пропускаем уже отформатированные строки
                if (line.StartsWith("•"))
                    continue;

                // 3. Пропускаем строки без цифр (кроме словесных количеств)
                bool hasDigit = Regex.IsMatch(line, @"\d");
                bool hasWordAmount = ContainsWordAmount(line);

                if (!hasDigit && !hasWordAmount)
                    continue;

                // 4. Словесные количества
                foreach (var kv in WordAmounts)
                {
                    if (line.Contains(kv.Key))
                    {
                        string name = ExtractNameBefore(line, kv.Key);
                        if (string.IsNullOrWhiteSpace(name))
                            name = "Ингредиент";

                        list.Add(new Ingredient
                        {
                            Name = Capitalize(name),
                            Weight = kv.Value,
                            OriginalText = raw.Trim()
                        });

                        goto NextLine;
                    }
                }

                // 5. Числовые форматы
                var match = Regex.Match(line,
                    @"^(?<name>.+?)\s*(?<amount>\d+([.,]\d+)?|\d+/\d+)\s*(?<unit>[а-яa-z\.]+)?$",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    string name = Capitalize(match.Groups["name"].Value.Trim());
                    string amountStr = NormalizeAmount(match.Groups["amount"].Value);
                    string unit = match.Groups["unit"].Value.Trim().ToLower();

                    double amount = ParseAmount(amountStr);
                    double weight = ConvertToGrams(amount, unit);

                    list.Add(new Ingredient
                    {
                        Name = name,
                        Weight = weight,
                        OriginalText = raw.Trim()
                    });
                }

            NextLine:
                continue;
            }

            return list;
        }

      

        private static bool ContainsWordAmount(string line)
        {
            foreach (var kv in WordAmounts)
                if (line.Contains(kv.Key))
                    return true;
            return false;
        }

        private static string ExtractNameBefore(string line, string key)
        {
            int idx = line.IndexOf(key);
            if (idx <= 0)
                return "";
            return line.Substring(0, idx)
                       .Replace("-", "")
                       .Replace("—", "")
                       .Trim();
        }

        private static string NormalizeAmount(string s)
        {
            return s.Replace(",", ".")
                    .Replace("½", "1/2")
                    .Replace("¼", "1/4")
                    .Replace("⅓", "1/3")
                    .Replace("⅔", "2/3");
        }

        private static double ParseAmount(string s)
        {
            if (s.Contains("/"))
            {
                var p = s.Split('/');
                double a, b;
                if (p.Length == 2 &&
                    double.TryParse(p[0], out a) &&
                    double.TryParse(p[1], out b))
                    return a / b;
            }

            double val;
            double.TryParse(s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out val);
            return val;
        }

        private static double ConvertToGrams(double amount, string unit)
        {
            switch (unit)
            {
                case "г":
                case "гр":
                case "грамм":
                case "грамма":
                    return amount;

                case "кг":
                    return amount * 1000;

                case "мл":
                    return amount;

                case "л":
                    return amount * 1000;

                case "шт":
                case "штук":
                    return amount * 50;

                case "ст.л":
                case "ст.л.":
                    return amount * 15;

                case "ч.л":
                case "ч.л.":
                    return amount * 5;

                case "стакан":
                    return amount * 250;

                default:
                    return amount;
            }
        }

        private static string Capitalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
