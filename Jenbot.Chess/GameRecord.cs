using Newtonsoft.Json;

namespace Jenbot.Chess;

public class GameRecord
{
    [JsonProperty("win")] 
    public int Wins { get; }
    
    [JsonProperty("loss")]
    public int Losses { get; }
    
    [JsonProperty("draw")]
    public int Draws { get; }
}