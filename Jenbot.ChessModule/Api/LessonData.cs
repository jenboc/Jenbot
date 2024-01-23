using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class LessonData
{
    [JsonProperty("highest")] public LessonStats? Highest { get; private set; }

    [JsonProperty("lowest")] public LessonStats? Lowest { get; private set; }
}