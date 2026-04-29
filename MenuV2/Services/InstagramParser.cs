using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MenuV2.Services
{
    public static class InstagramParser
    {
        private static readonly HttpClient client = new HttpClient();

        public class InstagramData
        {
            public string Title;
            public string PhotoUrl;
            public string IngredientsText;
            public string Instructions;
        }

        public static async Task<InstagramData> ParseAsync(string url)
        {
            // 1. Извлекаем shortcode
            var match = Regex.Match(url, @"instagram\.com\/p\/([^\/]+)");
            if (!match.Success)
                return null;

            string shortcode = match.Groups[1].Value;

            // 2. Запрос к API
            string apiUrl = "https://www.instagram.com/p/" + shortcode + "/?__a=1&__d=dis";
            string json = await client.GetStringAsync(apiUrl);

            // 3. Парсим JSON (старый using)
            using (var doc = JsonDocument.Parse(json))
            {
                var media = doc.RootElement
                    .GetProperty("graphql")
                    .GetProperty("shortcode_media");

                // CAPTION
                string caption =
                    media
                    .GetProperty("edge_media_to_caption")
                    .GetProperty("edges")[0]
                    .GetProperty("node")
                    .GetProperty("text")
                    .GetString();

                // PHOTO
                string photoUrl = media.GetProperty("display_url").GetString();

                InstagramData data = new InstagramData();
                data.Title = caption.Split('\n')[0].Trim();
                data.IngredientsText = caption.Trim();
                data.Instructions = caption.Trim();
                data.PhotoUrl = photoUrl;

                return data;
            }
        }
    }
}
