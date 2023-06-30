using Newtonsoft.Json;

namespace Jenbot.Chess;

public class BestGame
{
    [JsonProperty("date")]
    public int Date { get; }
    
    [JsonProperty("rating")]
    public int Rating { get; }
    
    [JsonProperty("game")]
    public string GameUrl { get; }
}
