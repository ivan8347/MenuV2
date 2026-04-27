using System;
using System.Text.RegularExpressions;
using MenuV2.Core;

namespace MenuV2.Services
{
    public static class InstagramParser
    {
        public class InstagramData
        {
            public string Title;
            public string PhotoUrl;
            public string IngredientsText;
            public string Instructions;
        }

        public static InstagramData Parse(string html)
        {
            var data = new InstagramData();

            // === 1. CAPTION (полный, без обрезания) ===
            // Ищем до следующей кавычки после text":
            var captionMatch = Regex.Match(
                html,
                "\"caption\":\\{\"text\":\"(.*?)\",\"",
                RegexOptions.Singleline
            );

            if (!captionMatch.Success)
                return null;

            // Декодируем JSON-экранирование
            string caption = Regex.Unescape(captionMatch.Groups[1].Value);

            // Декодируем HTML
            caption = System.Net.WebUtility.HtmlDecode(caption);

            // ВОССТАНАВЛИВАЕМ ПЕРЕНОСЫ
            caption = caption
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t");

            // === 2. TITLE ===
            data.Title = caption.Split('\n')[0].Trim();

            // === 3. ВЕСЬ caption → в Instructions ===
            data.Instructions = caption.Trim();

            // === 4. IngredientsText = весь caption ===
            data.IngredientsText = caption.Trim();

            return data;
        }
    }
}
