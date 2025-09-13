using Newtonsoft.Json;
using Jenbot.Utils;

namespace Jenbot.TriviaModule.Api;

public class TriviaQuestion
{
    [JsonProperty("category")]
    [JsonConverter(typeof(HtmlDecodeConverter))]
    public required string Category { get; set; }

    [JsonProperty("type")] 
    [JsonConverter(typeof(HtmlDecodeConverter))]
    public required string Type { get; set; }

    [JsonProperty("difficulty")] 
    [JsonConverter(typeof(HtmlDecodeConverter))]
    public required string Difficulty { get; set; }

    [JsonProperty("question")] 
    [JsonConverter(typeof(HtmlDecodeConverter))]
    public required string Question { get; set; }

    [JsonProperty("correct_answer")] 
    [JsonConverter(typeof(HtmlDecodeConverter))]
    public required string CorrectAnswer { get; set; }

    [JsonProperty("incorrect_answers")] 
    [JsonConverter(typeof(HtmlDecodeArrayConverter))]
    public required string[] IncorrectAnswers { get; set; }
}
