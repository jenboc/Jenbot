namespace Jenbot.Chess;

public class Player
{
    public Profile Profile { get; }
    public Stats Stats { get; }

    public Player(Profile profile, Stats stats)
    {
        Profile = profile;
        Stats = stats; 
    }
}