namespace Jenbot.Trivia;

internal class ApiResponse
{
    public int ResponseCode { get; private set; }
    public List<TriviaQuestion> Questions { get; private set; }
}