using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class GameRecord
{
    [JsonProperty("win")] public int Wins { get; private set; }

    [JsonProperty("loss")] public int Losses { get; private set; }

    [JsonProperty("draw")] public int Draws { get; private set; }
}