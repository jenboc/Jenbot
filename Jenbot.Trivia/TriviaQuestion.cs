using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Jenbot.Trivia;

public class TriviaQuestion
{
    [JsonProperty("category")]
    public string Category { get; private set; }
    [JsonProperty("type")]
    public string Type { get; private set; }
    [JsonProperty("difficulty")]
    public string Difficulty { get; private set; }
    [JsonProperty("question")]
    public string Question { get; private set; }
    [JsonProperty("correct_answer")]
    public string CorrectAnswer { get; private set; }
    [JsonProperty("incorrect_answers")]
    public string[] IncorrectAnswers { get; private set; }
}