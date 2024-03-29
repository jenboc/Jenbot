﻿using Newtonsoft.Json;

namespace Jenbot.ChessModule.Api;

public class LessonStats
{
    [JsonProperty("rating")] public int Rating { get; private set; }

    [JsonProperty("date")] public int Timestamp { get; private set; }
}