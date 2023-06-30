using Newtonsoft.Json;

namespace Jenbot.Chess;

public class Stats
{
    [JsonProperty("chess_daily")]
    public GameType? DailyChess { get; }
    
    [JsonProperty("chess_rapid")]
    public GameType? RapidChess { get; }
    
    [JsonProperty("chess_bullet")]
    public GameType? BulletChess { get; }
    
    [JsonProperty("chess_blitz")]
    public GameType? BlitzChess { get; }
    
    [JsonProperty("chess960_daily")]
    public GameType? Daily960Chess { get; }
}