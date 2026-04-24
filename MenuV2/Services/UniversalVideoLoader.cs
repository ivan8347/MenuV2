using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using MenuV2.Models;

public class UniversalVideoLoader
{
    private readonly YouTubeService _yt = new YouTubeService();
    private readonly VkVideoService _vk;
    private readonly InstagramService _insta = new InstagramService();

    public UniversalVideoLoader(string vkToken)
    {
        _vk = new VkVideoService(vkToken);
    }

    public async Task<VideoInfo> LoadAsync(string url)
    {
        url = url.ToLower();

        if (url.Contains("youtube.com") || url.Contains("youtu.be"))
            return await _yt.GetAsync(url);

        if (url.Contains("vk.com/video"))
            return await _vk.GetAsync(url);

        if (url.Contains("instagram.com"))
            return await _insta.GetAsync(url);

        throw new System.Exception("Неизвестная платформа");
    }
}
