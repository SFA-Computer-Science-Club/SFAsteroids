namespace SpaceGame.Code;

public class Player
{

    public int id { get; set; } = 0;

    public int points { get; set; } = 0;

    public void AddPoints(int amount)
    {
        points += amount;
    }
}