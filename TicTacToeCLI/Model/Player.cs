namespace TicTacToeCLI.Models;

public class Player
{
    private static int _numberOfPlayers;
    public List<IntegerPair> PlacedCurrently { get; }

    protected int NumberOfPlayers
    {
        get
        {
            if (_numberOfPlayers >= 2)
            {
                _numberOfPlayers = 0;
            }

            return _numberOfPlayers;
        }
        init => _numberOfPlayers = value;
    }

    public char Symbol { get; }
    protected string Name { get; init; }

    public Player(char symbol)
    {
        Symbol = symbol;
        {
            Name = $"Player {++NumberOfPlayers}";
        }
        PlacedCurrently = new List<IntegerPair>();
    }

    public override string ToString()
    {
        return string.Format(Name + " with symbol of '" + Symbol + "'");
    }

    public virtual void ResetData()
    {
        PlacedCurrently.Clear();
    }
    
}