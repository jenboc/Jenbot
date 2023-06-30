using Newtonsoft.Json;

namespace Jenbot.Chess;

public class LastGame
{
    [JsonProperty("date")]
    public int Date { get; }
    
    [JsonProperty("rating")]
    public int Rating { get; }
    
    [JsonProperty("rd")]
    public int Rd { get; }
}
