using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MenuV2.Models;

public class VkVideoService
{
    private readonly HttpClient _http = new HttpClient();
    private readonly string _token;

    public VkVideoService(string token)
    {
        _token = token;
    }

    public async Task<VideoInfo> GetAsync(string url)
    {
        var ids = ExtractVideoId(url); // ownerId_videoId

        var apiUrl =
            $"https://api.vk.com/method/video.get?videos={ids}&access_token={_token}&v=5.199";

        var json = await _http.GetStringAsync(apiUrl);
        using (var doc = JsonDocument.Parse(json))
        {

        var item = doc.RootElement
            .GetProperty("response")
            .GetProperty("items")[0];

        return new VideoInfo
        {
            Platform = "vk",
            Title = item.GetProperty("title").GetString(),
            Description = item.GetProperty("description").GetString(),
            ImageUrl = item.GetProperty("image")[0].GetProperty("url").GetString(),
            VideoUrl = item.GetProperty("player").GetString()
        };
        }
    }

    private string ExtractVideoId(string url)
    {
        // vk.com/video-123_456
        var match = System.Text.RegularExpressions.Regex.Match(url, @"video(-?\d+_\d+)");
        return match.Groups[1].Value;
    }
}
