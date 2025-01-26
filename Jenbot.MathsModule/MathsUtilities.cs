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
                new Dictionary<string,string>() { {"varwidth", ""}, {"border", "5pt"} })
        .AddPackage("amsfonts")
        .AddPackage("amsmath")
        .AddPackage("xcolor")
        .AddPreambleCommand("pagecolor", "0.1725490196,0.1843137255,0.2",
                new Dictionary<string,string>() { {"rgb", ""} })
        .AddPreambleCommand("color", "1,1,1",
                new Dictionary<string, string>() { {"rgb", ""} });
}
