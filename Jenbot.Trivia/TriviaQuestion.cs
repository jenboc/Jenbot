using System.Text.Json.Serialization;

namespace Jenbot.Trivia;

public class TriviaQuestion
{
    [JsonPropertyName("category")]
    public string Category { get; private set; }
    [JsonPropertyName("type")]
    public string Type { get; private set; }
    [JsonPropertyName("difficulty")]
    public string Difficulty { get; private set; }
    [JsonPropertyName("question")]
    public string Question { get; private set; }
    [JsonPropertyName("correct_answer")]
    public string CorrectAnswer { get; private set; }
    [JsonPropertyName("incorrect_answers")]
    public List<string> IncorrectAnswers { get; private set; }
}