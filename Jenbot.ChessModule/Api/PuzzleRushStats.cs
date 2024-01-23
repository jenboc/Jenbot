using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class PuzzleRushStats
{
    [JsonProperty("total_attempts")] public int TotalAttempts { get; private set; }

    [JsonProperty("score")] public int Score { get; private set; }
}