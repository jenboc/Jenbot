using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class Tournament
{
    [JsonProperty("count")] public int Count { get; private set; }

    [JsonProperty("withdraw")] public int Withdrawn { get; private set; }

    [JsonProperty("points")] public int TotalPoints { get; private set; }

    [JsonProperty("highest_finish")] public int HighestFinish { get; private set; }
}