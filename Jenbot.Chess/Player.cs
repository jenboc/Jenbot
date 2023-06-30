namespace Jenbot.Chess;

public class Player
{
    public Player(Profile profile, Stats stats)
    {
        Profile = profile;
        Stats = stats;
    }

    public Profile Profile { get; }
    public Stats Stats { get; }
}