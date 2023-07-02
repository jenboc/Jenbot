using System.Numerics;
using Discord;
using Discord.WebSocket;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;

namespace Jenbot.Commands;

public class InspirationalQuote : ICommand
{
    public string Name { get; }

    private const string DEFAULT_FONT = @"Resources\Fonts\cursive.ttf";
    private const string DEFAULT_BACKGROUND = @"Resources\Images\inspirational1.jpeg";
    private const int DEFAULT_PADDING = 10;

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
        var quote = (string)command.Data.Options.First().Value;

        var font = new Font(_defaultFontFamily, 50);
        var imagePath = DEFAULT_BACKGROUND; 
        var padding = DEFAULT_PADDING;
        var colour = Color.Red;
        var filepath = $"{Guid.NewGuid()}.jpeg";
        
        using (var img = await Image.LoadAsync(imagePath))
        {
            var textOptions = new TextOptions(font)
            {
                WrappingLength = img.Width - 2 * padding
            };

            var drawingOptions = new DrawingOptions()
            {
                Transform = Matrix3x2.CreateTranslation(padding, padding)
            };
            
            img.Mutate(i => 
                i.DrawText(drawingOptions, textOptions, quote, new SolidBrush(colour), new Pen(colour, 1f)));
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

        return new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Add an image to your very own inspirational quote")
            .AddOption(quote);
    }
}