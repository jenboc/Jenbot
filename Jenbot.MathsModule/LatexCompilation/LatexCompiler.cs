namespace Jenbot.MathsModule.LatexCompilation;

public static class LatexCompiler
{
    public static string Compile(LatexDocument latexDoc,
            string filename, CompilationTarget target)
    {
        return string.Empty;
    }

    public static string Compile(LatexDocumentBuilder latexDocBuilder,
            string filename, CompilationTarget target)
        => Compile(latexDocBuilder.Build(), filename, target);
}
