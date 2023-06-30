using Newtonsoft.Json;

namespace Jenbot.Chess;

public class Stats
{
    [JsonProperty("chess_daily")] public GameType? DailyChess { get; private set; }

    [JsonProperty("chess_rapid")] public GameType? RapidChess { get; private set; }

    [JsonProperty("chess_bullet")] public GameType? BulletChess { get; private set; }

    [JsonProperty("chess_blitz")] public GameType? BlitzChess { get; private set; }

    [JsonProperty("chess960_daily")] public GameType? Daily960Chess { get; private set; }

    [JsonProperty("tactics")] public LessonData? Tactics { get; private set; }

    [JsonProperty("lessons")] public LessonData? Lessons { get; private set; }

    [JsonProperty("puzzle_rush")] public PuzzleRushData? PuzzleRush { get; private set; }
}