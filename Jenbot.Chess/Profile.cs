using Newtonsoft.Json;

namespace Jenbot.Chess;

public class Profile
{
    [JsonProperty("@id")] public string Id { get; private set; }

    [JsonProperty("url")] public string Url { get; private set; }

    [JsonProperty("username")] public string Username { get; private set; }

    [JsonProperty("player_id")] public int PlayerId { get; private set; }

    [JsonProperty("title")] public string? Title { get; private set; }

    [JsonProperty("status")] public string Status { get; private set; }

    [JsonProperty("name")] public string? Name { get; private set; }

    [JsonProperty("avatar")] public string? AvatarUrl { get; private set; }

    [JsonProperty("location")] public string? Location { get; private set; }

    [JsonProperty("country")] public string CountryApi { get; private set; }

    [JsonProperty("joined")] public int RegistrationTimestamp { get; private set; }

    [JsonProperty("last_online")] public int LastOnlineTimestamp { get; private set; }

    [JsonProperty("followers")] public int Followers { get; private set; }

    [JsonProperty("is_streamer")] public bool IsStreamer { get; private set; }

    [JsonProperty("twitch_url")] public string? TwitchUrl { get; private set; }

    [JsonProperty("fide")] public int FideRating { get; private set; }

    [JsonProperty("verified")] public bool IsVerified { get; private set; }

    [JsonProperty("league")] public string League { get; private set; }
}