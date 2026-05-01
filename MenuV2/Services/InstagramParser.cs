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
        public static string NormalizeInstagramUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string url = input.Trim();
            url = url.Trim('"', '\'', ' ');
            url = url.Replace("\r", "").Replace("\n", "");

            if (!url.Contains("instagram.com"))
                return null;

            if (url.StartsWith("instagram.com"))
                url = "https://" + url;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "https://" + url;

            int q = url.IndexOf('?');
            if (q > 0)
            {
                string query = url.Substring(q);
                if (!query.Contains("igsh"))
                    url = url.Substring(0, q);
            }

            var m = Regex.Match(url, @"(reel|p)/([^/?#]+)");
            if (!m.Success)
                return null;

            string type = m.Groups[1].Value;
            string id = m.Groups[2].Value;

            return $"https://www.instagram.com/{type}/{id}/";
        }
        public static InstagramData ParseFromHtml(string html)
        {
            var data = new InstagramData();

            var captionMatch = Regex.Match(html, "\"caption\":\\{\"text\":\"(.*?)\",\"", RegexOptions.Singleline);
            if (captionMatch.Success)
            {
                string caption = Regex.Unescape(captionMatch.Groups[1].Value);
                caption = System.Net.WebUtility.HtmlDecode(caption);
                caption = caption.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");

                data.Title = caption.Split('\n')[0].Trim();
                data.Instructions = caption.Trim();
                data.IngredientsText = caption.Trim();
            }

            string photo = null;

            var m1 = Regex.Match(html, "\"display_url\":\"(https:[^\"]+)\"");
            if (m1.Success) photo = m1.Groups[1].Value;

            if (photo == null)
            {
                var m2 = Regex.Match(html, "\"url\":\"(https:[^\"]+)\"");
                if (m2.Success) photo = m2.Groups[1].Value;
            }

            if (photo == null)
            {
                var m3 = Regex.Match(html, "\"thumbnail_url\":\"(https:[^\"]+)\"");
                if (m3.Success) photo = m3.Groups[1].Value;
            }

            if (photo == null)
            {
                var m4 = Regex.Match(html, "<meta property=\"og:image\" content=\"(https:[^\"]+)\"");
                if (m4.Success) photo = m4.Groups[1].Value;
            }

            if (photo == null)
            {
                var m5 = Regex.Match(html, "<meta property=\"og:image:secure_url\" content=\"(https:[^\"]+)\"");
                if (m5.Success) photo = m5.Groups[1].Value;
            }

            if (photo == null)
            {
                var m6 = Regex.Match(html, "\"thumbnailUrl\"\\s*:\\s*\"(https:[^\"]+)\"");
                if (m6.Success) photo = m6.Groups[1].Value;
            }

            if (!string.IsNullOrEmpty(photo))
            {
                photo = photo.Replace("\\u0026", "&");
                photo = System.Net.WebUtility.HtmlDecode(photo);
                photo = photo.Trim('"', '\'', ' ');

                if (photo.StartsWith("/"))
                    photo = "https://www.instagram.com" + photo;

                if (Uri.TryCreate(photo, UriKind.Absolute, out var uri))
                    data.PhotoUrl = uri.ToString();
            }

            return data;
        }

    }
}
