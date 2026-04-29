using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MenuV2.Core;

namespace MenuV2.Services
{
    public static class IngredientParser
    {
        private static readonly Dictionary<string, double> WordAmounts = new Dictionary<string, double>
        {
            { "щепотка", 1 },
            { "щепотки", 1 },
            { "на кончике ножа", 1 },
            { "по вкусу", 1 },

            { "горсть", 30 },
            { "горсти", 30 },

            { "капля", 0.05 },
            { "капли", 0.05 },

            { "веточка", 3 },
            { "веточки", 3 },

            { "пучок", 30 },
            { "пучка", 30 },

            { "кусочек", 20 },
            { "кусочка", 20 }
        };

        public static List<Ingredient> FromText(string text)
        {
            var list = new List<Ingredient>();

            if (string.IsNullOrWhiteSpace(text))
                return list;

            text = Preprocess(text);

            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                string original = rawLine.Trim();
                string line = original.ToLower();

                if (line.Length < 2)
                    continue;

                // === 1. СЛОВЕСНЫЕ КОЛИЧЕСТВА ===
                foreach (var kv in WordAmounts)
                {
                    if (line.Contains(kv.Key))
                    {
                        int idx = line.IndexOf(kv.Key);
                        string name = line.Substring(0, idx).Trim();

                        if (string.IsNullOrWhiteSpace(name))
                            name = "ингредиент";

                        list.Add(new Ingredient
                        {
                            Name = Capitalize(name),
                            Weight = kv.Value,
                            OriginalText = original
                        });

                        goto NextLine;
                    }
                }

                // === 2. ЧИСЛО → ЕДИНИЦА → НАЗВАНИЕ ===
                var match = Regex.Match(line,
                    @"^(?<amount>\d+([.,]\d+)?|\d+/\d+|½|¼|⅓|⅔)\s*
                      (?<unit>г|гр|грамм|кг|мл|л|ст\.л\.?|ч\.л\.?|стакан|шт|пакет|упаковка|пучок|веточка|зубчик)?
                      \s*[-—]?\s*(?<name>.+)$",
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                // === 3. НАЗВАНИЕ → ЧИСЛО → ЕДИНИЦА ===
                if (!match.Success)
                {
                    match = Regex.Match(line,
                        @"^(?<name>.+?)\s*[-—]?\s*
                          (?<amount>\d+([.,]\d+)?|\d+/\d+|½|¼|⅓|⅔)\s*
                          (?<unit>г|гр|грамм|кг|мл|л|ст\.л\.?|ч\.л\.?|стакан|шт|пакет|упаковка|пучок|веточка|зубчик)?$",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                }

                if (match.Success)
                {
                    string name = Capitalize(match.Groups["name"].Value.Trim());
                    string amountStr = NormalizeAmount(match.Groups["amount"].Value);
                    string unit = match.Groups["unit"].Value.ToLower().Trim();

                    double amount = ParseAmount(amountStr);
                    double weight = ConvertToGrams(amount, unit);

                    var product = ProductStorage.FindSimilar(name);

                    Ingredient ing = product != null
                        ? new Ingredient(name, weight, product)
                        : new Ingredient { Name = name, Weight = weight };

                    ing.OriginalText = original;
                    list.Add(ing);

                    continue;
                }

            NextLine:
                continue;
            }

            return list;
        }

        // === ПРЕПРОЦЕССОР ===
        private static string Preprocess(string text)
        {
            var lines = text.Split('\n');
            var result = new List<string>();

            foreach (var raw in lines)
            {
                string l = raw.Trim();

                if (string.IsNullOrWhiteSpace(l))
                    continue;

                // игнорируем строки вида "(или 10 г сухих)"
                if (l.StartsWith("(") && l.EndsWith(")"))
                    continue;

                // убираем маркеры списка
                if (l.StartsWith("-"))
                    l = l.TrimStart('-', ' ');

                // убираем альтернативы в скобках
                l = Regex.Replace(l, @"\([^)]*\)", "").Trim();

                // нормализуем длинное тире
                l = l.Replace("—", "-");

                // нормализуем ложки
                l = l.Replace("ч. л.", "ч.л.")
                     .Replace("ст. л.", "ст.л.")
                     .Replace("ч.л ", "ч.л.")
                     .Replace("ст.л ", "ст.л.");

                result.Add(l);
            }

            return string.Join("\n", result);
        }

        private static string NormalizeAmount(string s)
        {
            return s
                .Replace("½", "1/2")
                .Replace("¼", "1/4")
                .Replace("⅓", "1/3")
                .Replace("⅔", "2/3")
                .Replace(',', '.');
        }

        private static double ParseAmount(string s)
        {
            if (s.Contains("/"))
            {
                var parts = s.Split('/');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0], out double a) &&
                    double.TryParse(parts[1], out double b))
                    return a / b;
            }

            double.TryParse(s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double result);

            return result;
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

                case "пакет":
                case "упаковка":
                    return amount * 100;

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
