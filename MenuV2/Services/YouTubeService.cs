using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MenuV2.Core;
using Newtonsoft.Json.Linq;

namespace MenuV2.Services
{
    public class YouTubeService
    {
        private readonly string _apiKey;

        public YouTubeService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public string ExtractVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            url = url.Trim();

            if (url.Contains("watch?v="))
                return url.Split(new[] { "watch?v=" }, StringSplitOptions.None)[1].Split('&')[0];

            if (url.Contains("youtu.be/"))
                return url.Split(new[] { "youtu.be/" }, StringSplitOptions.None)[1].Split('?')[0];

            return null;
        }

        public async Task<Recipe> LoadRecipeFromYoutube(string url)
        {
            string videoId = ExtractVideoId(url);
            if (videoId == null)
                return null;

            string apiUrl =
                $"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={_apiKey}&part=snippet";

            using (HttpClient client = new HttpClient())
            {
                string json;

                try
                {
                    json = await client.GetStringAsync(apiUrl);
                }
                catch
                {
                    return null;
                }

                var data = JObject.Parse(json);
                var snippet = data["items"]?[0]?["snippet"];

                if (snippet == null)
                    return null;

                string title = snippet["title"]?.ToString();
                string description = snippet["description"]?.ToString();

                // Скачиваем фото
                string photoPath = await DownloadThumbnail(videoId);

                // Парсим ингредиенты из описания
                var ingredients = IngredientParser.FromText(description);

                return new Recipe
                {
                    Name = title,
                    Instructions = description,
                    PhotoPath = photoPath,
                    VideoUrl = url,
                    Ingredients = ingredients
                };
            }
        }

        private async Task<string> DownloadThumbnail(string videoId)
        {
            string[] urls =
            {
                $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg",
                $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg"
            };

            using (HttpClient client = new HttpClient())
            {
                foreach (var url in urls)
                {
                    try
                    {
                        var bytes = await client.GetByteArrayAsync(url);

                        Directory.CreateDirectory("photos");

                        string filePath = Path.Combine("photos", $"youtube_{videoId}.jpg");

                        File.WriteAllBytes(filePath, bytes);

                        return filePath;
                    }
                    catch
                    {
                        // пробуем следующий URL
                    }
                }
            }

            return null;
        }
    }
}
