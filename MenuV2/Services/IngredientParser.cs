using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MenuV2.Core;

namespace MenuV2.Services
{
    public static class IngredientParser
    {
        // Словесные количества → граммы
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

            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim().ToLower();

                if (line.Length < 2)
                    continue;

                // 1) Проверяем словесные количества
                foreach (var kv in WordAmounts)
                {
                    if (line.Contains(kv.Key))
                    {
                        // Оригинальная строка
                        string original = rawLine.Trim();

                        // Название — всё ДО словесной меры
                        int idx = line.IndexOf(kv.Key);
                        string name = line.Substring(0, idx)
                                          .Replace("-", "")
                                          .Replace("—", "")
                                          .Trim();

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


                // 2) Универсальный Regex для числовых значений
                // Формат 1: "320 г муки"
                // Пробуем формат 1: "300 г муки"
                var match = Regex.Match(line,
                    @"^(?<amount>\d+([.,]\d+)?|\d+/\d+|½|¼|⅓|⅔)\s*(?<unit>г|гр|грамм|кг|мл|л|ст\.л\.|ст\.л|ч\.л\.|ч\.л|стакан|шт|пакет|упаковка|пучок|веточка|зубчик)?\s*(?<name>.+)$",
                    RegexOptions.IgnoreCase);

                // Пробуем формат 2: "мука 300 г"
                if (!match.Success)
                {
                    match = Regex.Match(line,
                        @"^(?<name>.+?)\s+(?<amount>\d+([.,]\d+)?|\d+/\d+|½|¼|⅓|⅔)\s*(?<unit>г|гр|грамм|кг|мл|л|ст\.л\.|ст\.л|ч\.л\.|ч\.л|стакан|шт|пакет|упаковка|пучок|веточка|зубчик)?$",
                        RegexOptions.IgnoreCase);
                }

                if (match.Success)
                {
                    string name = Capitalize(match.Groups["name"].Value.Trim());
                    string amountStr = NormalizeAmount(match.Groups["amount"].Value);
                    string unit = match.Groups["unit"].Value.ToLower().Trim();

                    double amount = ParseAmount(amountStr);
                    double weight = ConvertToGrams(amount, unit);

                    // Ищем продукт
                    var product = ProductStorage.FindSimilar(name);

                    Ingredient ing;

                    if (product != null)
                        ing = new Ingredient(name, weight, product);
                    else
                        ing = new Ingredient { Name = name, Weight = weight };

                    ing.OriginalText = rawLine.Trim();
                    list.Add(ing);

                    continue;
                }


            NextLine:
                continue;
            }

            return list;
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
                case "килограмм":
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
                case "ложка":
                case "ложки":
                    return amount * 15;

                case "ч.л":
                case "ч.л.":
                    return amount * 5;

                case "стакан":
                case "стакана":
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
