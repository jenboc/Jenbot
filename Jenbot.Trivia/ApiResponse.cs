using System.Text.Json.Serialization;

namespace Jenbot.Trivia;

internal class ApiResponse
{
    [JsonPropertyName("response_code")]
    public int ResponseCode { get; private set; }
    [JsonPropertyName("results")]
    public List<TriviaQuestion> Questions { get; private set; }
}