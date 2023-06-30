namespace Jenbot.Chess;

public class ChessApi
{
    private readonly HttpClient _httpClient;

    public ChessApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler()
        {
            UseProxy = false,
            Proxy = null
        });
    }
}