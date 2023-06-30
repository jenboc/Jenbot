using Newtonsoft.Json;

namespace Jenbot.Trivia;

internal class ApiResponse
{
    [JsonProperty("response_code")] public int ResponseCode { get; private set; }

    [JsonProperty("results")] public TriviaQuestion[] Questions { get; private set; }
}