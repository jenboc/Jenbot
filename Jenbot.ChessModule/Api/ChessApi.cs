using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class ChessApi
{
    private const string PROFILE_ENDPOINT = "https://api.chess.com/pub/player/USERNAME";
    private const string STATS_ENDPOINT = "https://api.chess.com/pub/player/USERNAME/stats";
    private const string USERNAME_PLACEHOLDER = "USERNAME";

    private const string DEFAULT_USER_AGENT =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0";

    private readonly HttpClient _httpClient;

    public ChessApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null
        });
    }

    private async Task<T?> GetDataAsync<T>(string endpoint)
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
            requestMessage.Headers.Add("User-Agent", DEFAULT_USER_AGENT);

            var raw = await (await _httpClient.SendAsync(requestMessage)).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(raw);
        }
        catch
        {
            return default;
        }
    }

    public async Task<Player?> GetPlayerAsync(string username)
    {
        var profile = await GetDataAsync<Profile>(PROFILE_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));
        var stats = await GetDataAsync<Stats>(STATS_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));

        return profile != null && stats != null ? new Player(profile, stats) : null;
    }
}