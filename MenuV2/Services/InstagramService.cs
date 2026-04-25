using System.Net.Http;
using System.Threading.Tasks;

public class InstagramService
{
    private readonly HttpClient _http;

    public InstagramService()
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
            "(KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
    }

    public async Task<string> GetHtmlAsync(string url)
    {
        return await _http.GetStringAsync(url);
    }
}
