using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class PuzzleRushData
{
    [JsonProperty("daily")] public PuzzleRushStats? Daily { get; private set; }

    [JsonProperty("best")] public PuzzleRushStats? Best { get; private set; }
}