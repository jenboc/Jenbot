using Jenbot.MathsModule.LatexCompilation;

namespace Jenbot.MathsModule;

internal static class MathsUtilities
{
    public async static Task<string> SaveImageAsync(IEnumerable<byte> imageBytes)
    {
        var guid = Guid.NewGuid();
        var filename = $"{guid}-tmp.jpg";

        await File.WriteAllBytesAsync(filename, imageBytes.ToArray());

        return filename;
    }

    public static LatexDocumentBuilder GetDefaultStandaloneLatexBuilder() => new LatexDocumentBuilder()
        .SetDocumentClass(LatexDocumentClass.Standalone, 
                new Dictionary<string,string>() { {"preview", ""} })
        .AddPackage("amsfonts")
        .AddPackage("amsmath");
}
