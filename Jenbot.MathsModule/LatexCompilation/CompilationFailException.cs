namespace Jenbot.MathsModule.LatexCompilation;

public class CompilationFailException : Exception
{
    public string PdfPath { get; }

    public CompilationFailException(string pdfPath)
    {
        PdfPath = pdfPath;
    }

    public CompilationFailException(string pdfPath, string msg) : base(msg)
    {
        PdfPath = pdfPath;
    }
}
