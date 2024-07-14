using TicTacToeCLI.Models;

namespace TicTacToeCLI.Model;

/// <summary>
/// The main class that represents the game. It contains the <see cref="GameGrid"/> object, list of <see cref="Players"/> participating in the game
/// and logic relating to the game itself.
/// </summary>
public class Game
{
    /// <summary>
    /// The side length of the game grid. This should always be 3 in TicTacToe, but can be changed for playing with bigger grids
    /// </summary>
    public const int GameGridSideLength = 3;

    private static readonly Random Random = new();

    /// <summary>
    /// The turn counter of the game. Starts at 1 and increments by 1 after each turn
    /// </summary>
    public int TurnCounter { get; private set; } = 1;

    /// <summary>
    /// The <see cref="Grid"/> object that represents the game board
    /// </summary>
    public Grid GameGrid { get; }

    /// <summary>
    /// The current <see cref="Player"/> of the game
    /// </summary>
    public Player CurrentPlayer { get; private set; }

    /// <summary>
    /// The list of players participating in the game. Declared as a read-only list
    /// </summary>
    protected IReadOnlyList<Player> Players { get; }

    /// <summary>
    /// Constructor for the <see cref="Game"/> class, which initializes the <see cref="GameGrid"/> and the <see cref="Players"/>
    /// </summary>
    /// <param name="player1">The first player to participate in the game, can only be a human player.</param>
    /// <param name="player2">The second player to participate in the game, can be a human or CPU player.</param>
    public Game(Player player1, Player player2)
    {
        GameGrid = new Grid(GameGridSideLength);
        Players = new[] { player1, player2 }.AsReadOnly();
        CurrentPlayer = ChooseRandomPlayer();
        NumberPlacement.SetGridSize(GameGridSideLength);
    }

    /// <summary>
    /// Checks if the current player has won the game
    /// </summary>
    /// <returns>Returns true, if the current player has won the game</returns>
    public bool CurrentPlayerHasWon()
    {
        return GameGrid.HasPlayerWon(CurrentPlayer);
    }

    /// <summary>
    /// Checks if the game is drawn
    /// </summary>
    /// <returns>Returns true, if all the positions in the <see cref="Grid"/> have been filled</returns>
    public bool GameIsDrawn()
    {
        return GameGrid.IsFull();
    }

    /// <summary>
    /// Changes the current player to the next player
    /// </summary>
    public void NextPlayer()
    {
        CurrentPlayer = CurrentPlayer == Players[0] ? Players[1] : Players[0];
    }

    /// <summary>
    /// Chooses a random player to start the game
    /// </summary>
    private Player ChooseRandomPlayer()
    {
        return Random.Next(2) == 0 ? Players[0] : Players[1];
    }

    /// <summary>
    /// Increases the turn counter of the game
    /// </summary>
    public void IncreaseTurnCounter()
    {
        TurnCounter++;
    }

    /// <summary>
    /// Resets the data of every <see cref="Player"/> and cpu involved in the game 
    /// </summary>
    private void ResetPlayerData()
    {
        foreach (var player in Players)
        {
            player.ClearData();
        }
    }

    /// <summary>
    /// Resets the game to the initial state
    /// This includes choosing a random player to start the game as well as resetting the <see cref="TurnCounter"/> and players' data
    /// </summary>
    public void ResetGame()
    {
        ResetPlayerData();
        GameGrid.ClearCells();
        CurrentPlayer = ChooseRandomPlayer();
        TurnCounter = 1;
    }
}