using Newtonsoft.Json;

namespace Jenbot.Chess;

public class Profile
{
    [JsonProperty("@id")]
    public string Id { get; }
    
    [JsonProperty("url")]
    public string Url { get; }
    
    [JsonProperty("username")]
    public string Username { get; }
    
    [JsonProperty("player_id")]
    public int PlayerId { get; }
    
    [JsonProperty("title")]
    public string? Title { get; }
    
    [JsonProperty("status")]
    public string Status { get; }
    
    [JsonProperty("name")]
    public string? Name { get; }
    
    [JsonProperty("avatar")]
    public string? AvatarUrl { get; }
    
    [JsonProperty("location")]
    public string? Location { get; }
    
    [JsonProperty("country")]
    public string CountryApi { get; }
    
    [JsonProperty("joined")]
    public int RegistrationTimestamp { get; }
    
    [JsonProperty("last_online")]
    public int LastOnlineTimestamp { get; }
    
    [JsonProperty("followers")]
    public int Followers { get; }
    
    [JsonProperty("is_streamer")]
    public bool IsStreamer { get; }
    
    [JsonProperty("twitch_url")]
    public string? TwitchUrl { get; }
    
    [JsonProperty("fide")]
    public int FideRating { get; }

    [JsonProperty("verified")]
    public bool IsVerified { get; }
    
    [JsonProperty("league")]
    public string League { get; }
}