using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MenuV2.Models;

public class YouTubeService
{
    private readonly HttpClient _http = new HttpClient();

    public async Task<VideoInfo> GetAsync(string url)
    {
        var oembedUrl = $"https://www.youtube.com/oembed?url={url}&format=json";
        var json = await _http.GetStringAsync(oembedUrl);

        using (var doc = JsonDocument.Parse(json))
        {

            var root = doc.RootElement;

        return new VideoInfo
        {
            Platform = "youtube",
            Title = root.GetProperty("title").GetString(),
            ImageUrl = root.GetProperty("thumbnail_url").GetString(),
            VideoUrl = url
        };
        }
    }
}
