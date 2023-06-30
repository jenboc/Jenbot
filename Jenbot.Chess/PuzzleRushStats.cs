using Newtonsoft.Json;

namespace Jenbot.Chess;

public class PuzzleRushStats
{
    [JsonProperty("total_attempts")]
    public int TotalAttempts { get; }
    
    [JsonProperty("score")]
    public int Score { get; }
}