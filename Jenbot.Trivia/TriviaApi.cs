using Newtonsoft.Json;

namespace Jenbot.Trivia;

public class TriviaApi
{
    private const string BASE_URL = "https://opentdb.com/api.php?amount=1";
    private readonly HttpClient _httpClient;

    public TriviaApi()
    {
        _httpClient = new HttpClient(new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null
        });
    }

    public async Task<TriviaQuestion?> GetQuestion()
    {
        var rawResponse = await _httpClient.GetStringAsync(BASE_URL);
        var parsedResponse = JsonConvert.DeserializeObject<ApiResponse>(rawResponse);

        return parsedResponse?.Questions.First();
    }
}