using System.Diagnostics;

namespace Jenbot.MathsModule.LatexCompilation;

public static class LatexCompiler
{
    public static string Compile(LatexDocument latexDoc,
            string filename, CompilationTarget target)
    {
        var pdfPath = CompileLatexToPdf(latexDoc, filename);

        // If we only wanted to get a PDF then we can stop here
        if (target == CompilationTarget.Pdf)
            return pdfPath;

        var jpegPath = ConvertPdfToJpeg(pdfPath, filename);
        
        // We do not need the PDF file anymore
        File.Delete(pdfPath);

        // We are done.
        return jpegPath;
    }

    public static string Compile(LatexDocumentBuilder latexDocBuilder,
            string filename, CompilationTarget target)
        => Compile(latexDocBuilder.Build(), filename, target);

    private static string CompileLatexToPdf(LatexDocument latexDoc, string filename)
    {
        var texPath = $"{filename}.tex";
        var pdfPath = $"{filename}.pdf";

        // First, we need to create a file containing the tex code
        File.WriteAllText(texPath, latexDoc.GetFileContents());

        // Then we need to use pdflatex to convert this into a PDF
        var procStartInfo = new ProcessStartInfo()
        {
            FileName = "pdflatex",
            Arguments = $"-interaction=nonstopmode {texPath}",
            CreateNoWindow = true,
            RedirectStandardError = true
        };
        var proc = Process.Start(procStartInfo);

        ArgumentNullException.ThrowIfNull(proc);

        proc.WaitForExit();

        var stderr = proc.StandardError.ReadToEnd();
        if (stderr.Length > 0)
            throw new CompilationFailException(stderr);

        // We can now delete the tex file, and the log and aux file generated in compilation
        File.Delete(texPath);
        File.Delete($"{filename}.log");
        File.Delete($"{filename}.aux");
        
        // We need to return the path to the PDF
        return pdfPath;
    }

    private static string ConvertPdfToJpeg(string pdfPath, string filename)
    {
        // Assumption: PDF already exists at that path, so we can just call
        // pdftocairo and be done
        var procStartInfo = new ProcessStartInfo()
        {
            FileName = "pdftocairo",
            Arguments = $"-r 300 -jpeg {pdfPath} {filename}",
            CreateNoWindow = true,
            RedirectStandardError = true
        };
        var proc = Process.Start(procStartInfo);

        ArgumentNullException.ThrowIfNull(proc);

        proc.WaitForExit();

        var stderr = proc.StandardError.ReadToEnd();
        if (stderr.Length > 0)
            throw new CompilationFailException(stderr);

        // Note: Since the PDF file already existed, we shouldn't delete it here
        // Now we return the path to jpeg
        return $"{filename}-1.jpg";
    }
}
