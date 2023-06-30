using Newtonsoft.Json;

namespace Jenbot.Chess;

public class ChessApi
{
    private readonly HttpClient _httpClient;

    private const string PROFILE_ENDPOINT = "https://api.chess.com/pub/player/USERNAME";
    private const string STATS_ENDPOINT = "https://api.chess.com/pub/player/USERNAME/stats";
    private const string USERNAME_PLACEHOLDER = "USERNAME";
    
    public ChessApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler()
        {
            UseProxy = false,
            Proxy = null
        });
    }

    private async Task<T?> GetDataAsync<T>(string endpoint)
    {
        Console.WriteLine(endpoint);
        return JsonConvert.DeserializeObject<T>(await _httpClient.GetStringAsync(endpoint));
    }

    public async Task<Player?> GetPlayerAsync(string username)
    {
        var profile = await GetDataAsync<Profile>(PROFILE_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));
        var stats = await GetDataAsync<Stats>(STATS_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));

        return profile != null && stats != null ? new Player(profile, stats) : null; 
    }
}