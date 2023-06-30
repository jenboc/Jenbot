using Newtonsoft.Json;

namespace Jenbot.Chess;

public class LessonData
{
    [JsonProperty("highest")]
    public LessonStats? Highest { get; }
    
    [JsonProperty("lowest")]
    public LessonStats? Lowest { get; }
}