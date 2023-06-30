using Newtonsoft.Json;

namespace Jenbot.Chess;

public class GameType
{
    [JsonProperty("last")]
    public LastGame? LastGame { get; }
    
    [JsonProperty("best")]
    public BestGame? BestGame { get; }
    
    [JsonProperty("record")]
    public GameRecord? Record { get; }
    
    [JsonProperty("tournament")]
    public Tournament? TournamentStats { get; }
}