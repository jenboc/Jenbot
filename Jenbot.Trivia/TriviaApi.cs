using System.Net.Http.Json;

namespace Jenbot.Trivia;

public class TriviaApi
{
    private const string BASE_URL = "https://opentdb.com/api.php?amount=1";
    private HttpClient _httpClient;
    
    public TriviaApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler()
        {
            UseProxy = false,
            Proxy = null
        });
    }

    public async Task<TriviaQuestion?> GetQuestion()
    {
        var rawResponse = await _httpClient.GetAsync(BASE_URL);
        var parsedResponse = await rawResponse.Content.ReadFromJsonAsync<ApiResponse>();

        return parsedResponse?.Questions.First();
    }
}