using Newtonsoft.Json;

namespace Jenbot.Chess;

public class ChessApi
{
    private readonly HttpClient _httpClient;

    private const string PROFILE_ENDPOINT = "https://api.chess.com/pub/player/USERNAME";
    private const string STATS_ENDPOINT = "https://api.chess.com/pub/player/USERNAME/stats";
    private const string USERNAME_PLACEHOLDER = "USERNAME";
    private const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0";
    
    public ChessApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler()
        {
            UseProxy = false,
            Proxy = null,
        });
    }

    private async Task<T?> GetDataAsync<T>(string endpoint)
    {
        Console.WriteLine(endpoint);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint); 
        requestMessage.Headers.Add("User-Agent", DEFAULT_USER_AGENT);
        var response = await _httpClient.SendAsync(requestMessage);
        var raw = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(raw);
        return JsonConvert.DeserializeObject<T>(raw);
    }

    public async Task<Player?> GetPlayerAsync(string username)
    {
        /*var profile = await GetDataAsync<Profile>(PROFILE_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));
        var stats = await GetDataAsync<Stats>(STATS_ENDPOINT.Replace(USERNAME_PLACEHOLDER, username));*/

        var profile = await GetDataAsync<Profile>("https://api.chess.com/pub/player/abyssmisty");
        var stats = await GetDataAsync<Stats>("https://api.chess.com/pub/player/abyssmisty/stats");
        
        return profile != null && stats != null ? new Player(profile, stats) : null; 
    }
}