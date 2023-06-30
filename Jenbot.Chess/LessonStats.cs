using Newtonsoft.Json;

namespace Jenbot.Chess;

public class LessonStats
{
    [JsonProperty("rating")]
    public int Rating { get; }
    
    [JsonProperty("date")]
    public int Timestamp { get; }
}