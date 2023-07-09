using System.Numerics;
using Discord;
using Discord.WebSocket;
using SixLabors.Fonts;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Drawing.Processing;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;

namespace Jenbot.Commands;

public class InspirationalQuote : ICommand
{
    public string Name { get; }

    private const string DEFAULT_FONT = @"Resources/Fonts/DancingScript.ttf";
    private const string DEFAULT_BACKGROUND = @"Resources/Images/inspirational1.jpeg";
    private const int DEFAULT_PADDING = 10;
    private static readonly Color _defaultColour = Color.Black;
    
    private FontCollection _fontCollection;
    private FontFamily _defaultFontFamily;
    
    public InspirationalQuote()
    {
        Name = "inspirational-quote";
        _fontCollection = new FontCollection();
        _defaultFontFamily = _fontCollection.Add(DEFAULT_FONT);
    }

    public async Task Execute(SocketSlashCommand command)
    {
        var options = new CommandOptions(command.Data.Options);
        
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
            
            img.Mutate(i => 
                i.DrawText(drawingOptions, textOptions, options.Quote, new SolidBrush(options.Colour), new Pen(options.Colour, 1f)));
            await img.SaveAsync(filepath);
        }

        await command.RespondWithFileAsync(filepath);
        File.Delete(filepath);
    }

    public SlashCommandBuilder GetCommandBuilder()
    {
        var quote = new SlashCommandOptionBuilder()
            .WithName("quote")
            .WithDescription("The quote")
            .WithRequired(true)
            .WithType(ApplicationCommandOptionType.String);

        var colour = new SlashCommandOptionBuilder()
            .WithName("colour")
            .WithDescription("Hex code for the colour of the text")
            .WithType(ApplicationCommandOptionType.String);

        return new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Add an image to your very own inspirational quote")
            .AddOption(quote)
            .AddOption(colour);
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

    struct CommandOptions
    {
        public string? Quote { get; }
        public Color Colour { get; set; }

        public CommandOptions(IReadOnlyCollection<SocketSlashCommandDataOption> options)
        {
            Colour = _defaultColour; 
            
            foreach (var option in options)
            {
                switch (option.Name)
                {
                    case "quote":
                        Quote = (string)option.Value;
                        break;
                    case "colour":
                        Colour = ConvertColour((string)option.Value);
                        break;
                }
            }
        }
    }
}