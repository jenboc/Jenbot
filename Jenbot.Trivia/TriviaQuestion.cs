namespace Jenbot.Trivia;

public class TriviaQuestion
{
    public string Category { get; private set; }
    public string Type { get; private set; }
    public string Difficulty { get; private set; }
    public string Question { get; private set; }
    public string CorrectAnswer { get; private set; }
    public List<string> IncorrectAnswers { get; private set; }
}