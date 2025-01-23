namespace Jenbot.MathsModule.LatexCompilation;

public class CompilationFailException : Exception
{
    public CompilationFailException()
    {
    }

    public CompilationFailException(string msg) : base(msg)
    {
    }
}
