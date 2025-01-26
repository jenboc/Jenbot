namespace Jenbot.MathsModule.LatexCompilation;

public static class LatexUtilities
{
    private static List<string> _unsafeCommands = new()
    {
        "input",
        "immediate",
        "write18"
    };

    public static bool IsCodeSnippetSafe(string snippet)
        => !SnippetContainsUnsafeCommand(snippet);

    private static bool SnippetContainsUnsafeCommand(string snippet)
    {
        foreach (var comm in _unsafeCommands)
        {
            if (snippet.Contains($"\\{comm}"))
                return true;
        }

        return false;
    }
}
