using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class BestGame
{
    [JsonProperty("date")] public int Date { get; private set; }

    [JsonProperty("rating")] public int Rating { get; private set; }

    [JsonProperty("game")] public string GameUrl { get; private set; }
}