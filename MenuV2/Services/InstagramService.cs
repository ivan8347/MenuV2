using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MenuV2.Models;

public class InstagramService
{
    private readonly HttpClient _http;

    public InstagramService()
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
    }

    public async Task<VideoInfo> GetAsync(string url)
    {
        var html = await _http.GetStringAsync(url);

        var ld = ExtractLdJson(html);
        if (ld != null)
        {
            return new VideoInfo
            {
                Platform = "instagram",
                Title = null,
                Description = null,
                VideoUrl = ld.VideoUrl,
                ImageUrl = ld.ImageUrl
            };
        }

        var shared = ExtractSharedData(html);
        if (shared != null)
        {
            return new VideoInfo
            {
                Platform = "instagram",
                Title = null,
                Description = null,
                VideoUrl = shared.VideoUrl,
                ImageUrl = shared.ImageUrl
            };
        }

        throw new Exception("Не удалось извлечь данные Instagram");
    }

    private InstagramMediaResult ExtractLdJson(string html)
    {
        var match = Regex.Match(html,
            "<script type=\"application/ld\\+json\">(.*?)</script>",
            RegexOptions.Singleline);

        if (!match.Success)
            return null;

        var json = match.Groups[1].Value;

        var video = Regex.Match(json, "\"contentUrl\":\"(https:[^\"]+\\.mp4)\"");
        var image = Regex.Match(json, "\"thumbnailUrl\":\"(https:[^\"]+\\.jpg)\"");

        return new InstagramMediaResult
        {
            VideoUrl = video.Success ? video.Groups[1].Value : null,
            ImageUrl = image.Success ? image.Groups[1].Value : null
        };
    }

    private InstagramMediaResult ExtractSharedData(string html)
    {
        var match = Regex.Match(html,
            "window\\.__additionalDataLoaded\\('.*?',(.*?)\\);",
            RegexOptions.Singleline);

        if (!match.Success)
            return null;

        var json = match.Groups[1].Value;

        var video = Regex.Match(json, "\"video_url\":\"(https:[^\"]+\\.mp4)\"");
        var image = Regex.Match(json, "\"display_url\":\"(https:[^\"]+\\.jpg)\"");

        return new InstagramMediaResult
        {
            VideoUrl = video.Success ? video.Groups[1].Value.Replace("\\u0026", "&") : null,
            ImageUrl = image.Success ? image.Groups[1].Value.Replace("\\u0026", "&") : null
        };
    }
}

public class InstagramMediaResult
{
    public string VideoUrl { get; set; }
    public string ImageUrl { get; set; }
}
