namespace SpaceGame.Code;

public class Player
{

    public int ID { get; set; } = 0;
    public string PlayerName { get; set; } = "Player 1";

    public int Points { get; private set; } = 0;

    public void AddPoints(int amount)
    {
        Points += amount;
    }
}