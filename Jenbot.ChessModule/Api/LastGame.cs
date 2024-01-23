using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class LastGame
{
    [JsonProperty("date")] public int Date { get; private set; }

    [JsonProperty("rating")] public int Rating { get; private set; }

    [JsonProperty("rd")] public int Rd { get; private set; }
}