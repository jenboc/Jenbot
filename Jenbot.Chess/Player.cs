namespace Jenbot.Chess;

public class Player
{
    public Profile Profile { get; }
    public Stats Stats { get; }
    public bool CurrentlyOnline { get; }

    public Player(Profile profile, Stats stats, bool currentlyOnline)
    {
        Profile = profile;
        Stats = stats;
        CurrentlyOnline = currentlyOnline;
    }
}