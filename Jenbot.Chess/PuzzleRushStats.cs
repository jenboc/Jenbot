using Newtonsoft.Json;

namespace Jenbot.Chess;

public class PuzzleRushStats
{
    [JsonProperty("total_attempts")] public int TotalAttempts { get; private set; }

    [JsonProperty("score")] public int Score { get; private set; }
}