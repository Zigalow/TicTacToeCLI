namespace TicTacToeCLI.Model;

/// <summary>
/// This class represents a player.
/// It contains a list of <see cref="SymbolPositions"/>, methods for retrieving the list and adding an <see cref="IntegerPair"/> to the list,
/// as well as a <see cref="Name"/> and <see cref="Symbol"/> property.
/// </summary>
/// <remarks>This class should be inherited by a computer player.</remarks>
public class Player
{
    /// <summary>
    /// Represents the current positions of a player's placements of symbols in the game.
    /// </summary>
    private readonly List<IntegerPair> _symbolPositions;

    private static int _numberOfPlayers;

    /// <summary>
    /// The chosen symbol of the player
    /// </summary>
    public char Symbol { get; }

    /// <summary>
    /// Property that returns the number of players in the game. It's designed to not be more than two players.
    /// </summary>

    private static int NumberOfPlayers
    {
        get
        {
            if (_numberOfPlayers >= 2)
            {
                _numberOfPlayers = 0;
            }

            return ++_numberOfPlayers;
        }
    }

    /// <summary>
    /// Readonly list of the player's symbol positions in the game/>
    /// </summary>
    public IReadOnlyList<IntegerPair> SymbolPositions => _symbolPositions.AsReadOnly();

    /// <summary>
    /// The chosen name of the player
    /// </summary>
    private string Name { get; }

    /// <summary>
    /// Constructor for <see cref="Player"/> class. Chained to the protected constructor, where null is passed in as a name, to get generic name for the player
    /// </summary>
    /// <param name="symbol">Displayed symbol of player</param>
    public Player(char symbol) : this(null, symbol)
    {
    }
    
    /// <summary>
    /// Protected constructor for <see cref="Player"/> class.
    /// Currently intended to give generic name to local players, where null is passed in as name in the chained constructor
    /// </summary>
    /// <param name="name">Displayed name of player</param>
    /// <param name="symbol">Displayed symbol of player</param>
    protected Player(string? name, char symbol)
    {
        Symbol = symbol;
        Name = string.IsNullOrEmpty(name) ? $"Player {NumberOfPlayers}" : name;
        _symbolPositions = new List<IntegerPair>();
    }

    

    /// <summary>
    /// Adds an <see cref="IntegerPair"/> to the list of <see cref="SymbolPositions"/>, representing that a symbol has been placed by a player
    /// </summary>
    /// <param name="pair">the integer pair being added to the <see cref="SymbolPositions"/> list</param>
    public void AddSymbolPosition(IntegerPair pair)
    {
        _symbolPositions.Add(pair);
    }

    /// <summary>
    /// Clears the data of the player. This includes clearing the list of <see cref="SymbolPositions"/>
    /// </summary>
    public virtual void ClearData()
    {
        _symbolPositions.Clear();
    }

    /// <summary>
    /// Returns a string that represents the <see cref="Name"/> and <see cref="Symbol"/> of the player.
    /// </summary>
    /// <returns>a string that represents the <see cref="Name"/> and <see cref="Symbol"/> of the player</returns>
    public override string ToString()
    {
        return $"{Name} with symbol of '{Symbol}'";
    }
}