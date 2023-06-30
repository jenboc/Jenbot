using Newtonsoft.Json;

namespace Jenbot.Chess;

public class GameType
{
    [JsonProperty("last")]
    public LastGame? LastGame { get; private set; }
    
    [JsonProperty("best")]
    public BestGame? BestGame { get; private set; }
    
    [JsonProperty("record")]
    public GameRecord? Record { get; private set; }
    
    [JsonProperty("tournament")]
    public Tournament? TournamentStats { get; private set; }
}