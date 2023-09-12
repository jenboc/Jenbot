using System.Numerics;
using Discord.Interactions;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Jenbot.Modules;

public class ImageCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly Color _defaultColour = Color.Black;
    private const string DEFAULT_FONT = @"Resources/Fonts/DancingScript.ttf";
    private const string DEFAULT_BACKGROUND = @"Resources/Images/inspirational1.jpeg";
    private const int DEFAULT_PADDING = 10;
    
    private static FontCollection _fontCollection;
    private static FontFamily _defaultFontFamily;

    static ImageCommandModule()
    {
        _fontCollection = new();
        _defaultFontFamily = _fontCollection.Add(DEFAULT_FONT);
    }
    
    [SlashCommand("inspirational-quote", "Add an image to your very own inspirational quote")]
    public async Task InspirationalQuote(string quote, string fontHexCode="000000")
    {
        await DeferAsync();
        
        var font = new Font(_defaultFontFamily, 100);
        var imagePath = DEFAULT_BACKGROUND; 
        var padding = DEFAULT_PADDING;
        var filepath = $"{Guid.NewGuid()}.jpeg";
        
        using (var img = await Image.LoadAsync(imagePath))
        {
            var textOptions = new TextOptions(font)
            {
                WrappingLength = img.Width - 2 * padding,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var drawingOptions = new DrawingOptions()
            {
                Transform = Matrix3x2.CreateTranslation(img.Width / 2f, padding)
            };

            var colour = ConvertColour(fontHexCode);
            
            img.Mutate(i => 
                i.DrawText(drawingOptions, textOptions, quote, new SolidBrush(colour), new Pen(colour, 1f)));
            await img.SaveAsync(filepath);
        }

        await FollowupWithFileAsync(filepath);
        File.Delete(filepath);
    }
    
    private static Color ConvertColour(string hex)
    {
        // Invalid hexcode given
        if (hex.Length != 6)
            return _defaultColour;

        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);

        var rgb = new Rgb24((byte)Convert.ToInt32(r, 16),
            (byte)Convert.ToInt32(g, 16),
            (byte)Convert.ToInt32(b, 16));

        return new Color(rgb);
    }
}