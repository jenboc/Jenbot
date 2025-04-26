using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

using Jenbot.Interactions;

namespace Jenbot.GamesModule.NoughtsAndCrosses;

public class Game
{
    private Board _board;

    private static readonly Dictionary<Tile, Button.Colour> TILE_COLOURS = new()
    {
        { Tile.Cross, Button.Colour.Red },
        { Tile.Nought, Button.Colour.Green },
        { Tile.Empty, Button.Colour.Grey }
    };

    private Dictionary<Tile, DiscordUser> _players;
    private Tile _currentPlayer;

    private InteractionContext? _ctx;
    private ulong? _messageId;

    public Game(DiscordUser crosses, DiscordUser noughts, int boardSize = 3, int maxTiles = 0)
    {
        // Validate players
        if (crosses == noughts)
        {
            throw new ArgumentOutOfRangeException("You cannot play against yourself");
        }

        if (crosses.IsBot || noughts.IsBot)
        {
            throw new ArgumentOutOfRangeException("Bots cannot play");
        }

        _board = new Board(boardSize, maxTiles);

        _players = new()
        {
            { Tile.Nought, noughts },
            { Tile.Cross, crosses }
        };
    }

    private void NextPlayer()
    {
        _currentPlayer = _currentPlayer == Tile.Cross 
            ? Tile.Nought 
            : Tile.Cross;
    }

    public async Task Start(InteractionContext ctx)
    {
        var message = await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("# Game will start soon"));

        _ctx = ctx;
        _messageId = message.Id;

        await UpdateGame();
    }

    public IEnumerable<DiscordActionRowComponent> CreateButtonRows(bool areEnabled)
    {
        var row = new List<Button>();

        for (var i = 0; i < _board.Size; i++)
        {
            row.Clear();

            for (var j = 0; j < _board.Size; j++)
            {
                row.Add(TileToButton(i, j, areEnabled));
            }

            var rowComponents = row.Select(b => b.GetComponent());
            yield return new DiscordActionRowComponent(rowComponents);
        }
    }

    private Button TileToButton(int i, int j, bool isEnabled)
    {
        if (i < 0 || i >= _board.Size || j < 0 || j >= _board.Size)
            throw new ArgumentOutOfRangeException("Coordinates out of range");

        var colour = TILE_COLOURS[_board[i,j]];
        var label = _board[i,j] switch
        {
            Tile.Cross => "X",
            Tile.Nought => "O",
            _ => "-"
        };

        var button = new Button(
            label,
            colour: colour,
            enabled: isEnabled
        );

        if (_board[i,j] == Tile.Empty && isEnabled)
        {
            button.OnButtonClick += async (s, e) => await PlaceTile(i, j, e.Interaction);
        }

        return button;
    }

    // Button call back for board buttons
    private async Task PlaceTile(int i, int j, DiscordInteraction interaction)
    {
        // Did the correct player press the button?
        if (interaction.User != _players[_currentPlayer])
            return;

        _board.TryPlaceTile(_currentPlayer, i, j);
        
        await interaction.DeferAsync(true);

        NextPlayer();
        await UpdateGame();

        await interaction.DeleteOriginalResponseAsync();
    }

    private async Task UpdateGame()
    {
        var state = _board.EvaluateState();
        var message = state switch
        {
            GameState.Ongoing => $"{_players[_currentPlayer].Mention}'s Turn",
            GameState.Draw => $"Draw!",
            GameState.CrossWin => $"{_players[Tile.Cross].Mention} Wins!",
            GameState.NoughtsWin => $"{_players[Tile.Nought].Mention} Wins!",
            _ => "idk what's happening"
        };

        var actionRows = CreateButtonRows(state == GameState.Ongoing);

        await _ctx!.EditFollowupAsync(
                _messageId!.Value,
                new DiscordWebhookBuilder().WithContent($"# {message}").AddComponents(actionRows)
        );
    }
}
