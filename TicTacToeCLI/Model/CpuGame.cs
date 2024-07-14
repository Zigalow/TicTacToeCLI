using System.Collections.ObjectModel;

namespace TicTacToeCLI.Model;

/// <summary>
/// The CpuGame class inherits from the Game class and represents a game of Tic Tac Toe between a human player and a CPU player.
/// </summary>
/// <remarks>
/// This class contains methods to determine if the CPU player can win or lose in the next move. It also generates all possible grid positions at the start of the game.
/// </remarks>
public class CpuGame : Game
{

    /// <summary>
    /// Constructor for the CpuGame class. Initializes the human player and the CPU player,
    /// and sets the name of the human player to "Player 1".
    /// </summary>
    /// <param name="player1"> The human player participating in the game.</param>
    /// <param name="cpu"> The CPU player participating in the game.</param>
    public CpuGame(Player player1, Cpu cpu) : base(player1, cpu)
    {
        Cpu = cpu;
        player1.SetName("Player 1");
    }

    /// <summary>
    /// Gets a read-only list of all possible grid positions in the game.
    /// </summary>
    /// <remarks>
    /// This list contains all valid coordinate pairs (represented as IntegerPair objects)
    /// for the game grid. It is generated once at startup based on the GameGridSideLength
    /// and remains constant throughout the game. Players can choose their moves from these positions.
    /// </remarks>
    /// <value>
    /// An <see cref="IReadOnlyList{T}"/> of <see cref="IntegerPair"/> objects, where each <see cref="IntegerPair"/> represents
    /// a valid (row, column) coordinate on the game grid.
    /// </value>
    public static IReadOnlyList<IntegerPair> AllGridPositions { get; } =
        GenerateAllGridPositions(GameGridSideLength);

    private Cpu Cpu { get; }

    /// <summary>
    /// Determines if the CPU player can win in the next move.
    /// </summary>
    /// <param name="winningPair">When this method returns, contains the <see cref="IntegerPair"/> representing 
    /// the position where the CPU should place its symbol to win, if a winning move is possible; otherwise, null.</param>
    /// <returns>
    /// <c>true</c> if the CPU can win with its next move; otherwise, <c>false</c>.
    /// </returns>
    public bool CpuCanWin(out IntegerPair? winningPair)
    {
        WinPossibility winPossibility = new(Cpu.SymbolPositions, GameGridSideLength);
        return CalculateWinPossibility(winPossibility, out winningPair);
    }

    /// <summary>
    /// Determines if the human player can win in their next move, which would result in the CPU losing.
    /// </summary>
    /// <param name="pairToPreventLoss">When this method returns, contains the <see cref="IntegerPair"/> representing 
    /// the position where the CPU should place its symbol to prevent losing, if the human player has a winning move; 
    /// otherwise, null.</param>
    /// <returns>
    /// <c>true</c> if the human player can win (and thus the CPU can lose) in the next move; otherwise, <c>false</c>.
    /// </returns>
    public bool CpuCanLose(out IntegerPair? pairToPreventLoss)
    {
        WinPossibility winPossibility = new(Players[0].SymbolPositions, GameGridSideLength);
        return CalculateWinPossibility(winPossibility, out pairToPreventLoss);
    }

    private static ReadOnlyCollection<IntegerPair> GenerateAllGridPositions(int gridSideLength)
    {
        var tempArr = new List<IntegerPair>(gridSideLength * gridSideLength);

        for (int i = 0; i < gridSideLength; i++)
        {
            for (int j = 0; j < gridSideLength; j++)
            {
                tempArr.Add(new IntegerPair(i, j));
            }
        }

        return tempArr.AsReadOnly();
    }

    /// <summary>
    /// Calculates whether it's possible for the player to win based on the given <see cref="WinPossibility"/>.
    /// </summary>
    /// <param name="winPossibility">The <see cref="WinPossibility"/> of the player to calculate win potential for.</param>
    /// <param name="pair">When this method returns, contains the <see cref="IntegerPair"/> representing the position 
    /// where the player should place their symbol to win, if a winning move is possible; otherwise, null.</param>
    /// <returns>
    /// <c>true</c> if it's possible for the player to win with their next move; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method checks for winning possibilities in the following order:
    /// 1. Horizontally (referred to as "Sideways" in the code)
    /// 2. Vertically
    /// 3. Diagonally (referred to as "Crosses" in the code)
    ///    - Left-to-right diagonal
    ///    - Right-to-left diagonal
    /// The method stops and returns true as soon as it finds a winning move.
    /// </remarks>
    private bool CalculateWinPossibility(WinPossibility winPossibility, out IntegerPair? pair)
    {
        // Sideways
        foreach (var side in winPossibility.Sides)
        {
            for (int i = 0; i < GameGridSideLength; i++)
            {
                if (GameGrid[i, side] != null)
                {
                    continue;
                }

                pair = new IntegerPair(i, side);
                return true;
            }
        }

        // Vertically
        foreach (var vert in winPossibility.Vertically)
        {
            for (int i = 0; i < GameGridSideLength; i++)
            {
                if (GameGrid[vert, i] == null)
                {
                    pair = new IntegerPair(vert, i);
                    return true;
                }
            }
        }

        // Crosses

        //Left_to_Right then Right_to_Left crosses
        for (var i = 0; i < 2; i++)
        {
            // First index is true, if a win-possibility is present for left_to_right cross. Second index is for right_to_left cross
            if (!winPossibility.Crosses[i])
            {
                continue;
            }

            if (i == 0) // If there is a win-possibility with the left_to_right cross
            {
                for (var j = 0; j < GameGridSideLength; j++)
                {
                    if (GameGrid[j, j] != null)
                    {
                        continue;
                    }

                    pair = new IntegerPair(j, j);
                    return true;
                }
            }
            else // There is a win-possibility with the right_to_left cross
            {
                for (var j = 0; j < GameGridSideLength; j++)
                {
                    if (GameGrid[GameGridSideLength - j - 1, j] != null)
                    {
                        continue;
                    }

                    pair = new IntegerPair(GameGridSideLength - j - 1, j);
                    return true;
                }
            }
        }

        pair = null;
        return false;
    }
}