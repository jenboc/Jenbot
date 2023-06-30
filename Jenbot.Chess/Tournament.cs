using Newtonsoft.Json;

namespace Jenbot.Chess;

public class Tournament
{
    [JsonProperty("count")]
    public int Count { get; }
    
    [JsonProperty("withdraw")]
    public int Withdrawn { get; }
    
    [JsonProperty("points")]
    public int TotalPoints { get; }
    
    [JsonProperty("highest_finish")]
    public int HighestFinish { get; }
}