using System.Collections;
using System.Text;

namespace TicTacToeCLI.Model;

/// <summary>
/// Represents a grid for the Tic Tac Toe game.
/// </summary>
public class Grid : IEnumerable
{
    private readonly char?[,] _gridFormat;
    private readonly int _sideLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="Grid"/> class with the specified size.
    /// </summary>
    /// <param name="size">The side length of the grid. Must be an odd number.</param>
    /// <exception cref="EvenGridSizeException">Thrown when the size is an even number.</exception>
    public Grid(int size)
    {
        if (size % 2 == 0)
        {
            throw new EvenGridSizeException();
        }

        _gridFormat = new char?[size, size];
        _sideLength = size;
    }

    /// <summary>
    /// Gets or sets the value at the specified row and column indices.
    /// </summary>
    /// <param name="index1">The row index.</param>
    /// <param name="index2">The column index.</param>
    /// <returns>The value at the specified indices.</returns>
    public char? this[int index1, int index2]
    {
        get => _gridFormat[index1, index2];
        set => _gridFormat[index1, index2] = value;
    }

    /// <summary>
    /// Gets or sets the value at the specified position represented by an <see cref="IntegerPair"/>.
    /// </summary>
    /// <param name="pair">The position represented by an <see cref="IntegerPair"/>.</param>
    /// <returns>The value at the specified position.</returns>
    public char? this[IntegerPair pair]
    {
        get => _gridFormat[pair.First, pair.Second];
        set => _gridFormat[pair.First, pair.Second] = value;
    }

    /// <summary>
    /// Determines whether the specified position is available.
    /// </summary>
    /// <param name="in1">The row index.</param>
    /// <param name="in2">The column index.</param>
    /// <returns><c>true</c> if the position is available; otherwise, <c>false</c>.</returns>
    public bool IsSpaceAvailable(int in1, int in2)
    {
        return _gridFormat[in1, in2] == null;
    }

    /// <summary>
    /// Determines whether the specified position represented by an <see cref="IntegerPair"/> is available.
    /// </summary>
    /// <param name="pair">The position represented by an <see cref="IntegerPair"/>.</param>
    /// <returns><c>true</c> if the position is available; otherwise, <c>false</c>.</returns>
    public bool IsSpaceAvailable(IntegerPair pair)
    {
        return this[pair] != null;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the grid.
    /// </summary>
    /// <returns>An enumerator for the grid.</returns>
    public IEnumerator GetEnumerator()
    {
        return _gridFormat.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the grid.
    /// </summary>
    /// <returns>An enumerator for the grid.</returns>
    public bool IsAllCellsFilled()
    {
        foreach (var cell in _gridFormat)
        {
            if (cell == null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified player has won the game.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns><c>true</c> if the player has won; otherwise, <c>false</c>.</returns>
    public bool HasPlayerWon(Player player)
    {
        char symbolToCheckFor = player.Symbol;
        return PlayerHasWonVerticallyOrHorizontally(symbolToCheckFor) || PlayerHasWonDiagonally(symbolToCheckFor);
    }

    /// <summary>
    /// Returns a string representation of the grid.
    /// This method is responsible for generating the grid layout in the TUI.
    /// </summary>
    /// <returns>A string representation of the grid.</returns>
    public override string ToString()
    {
        StringBuilder layout = new();

        string standardLine = "+" + new StringBuilder(4 * _sideLength).Insert(0, "-----+", _sideLength);

        for (int i = 0; i < _sideLength; i++)
        {
            layout.AppendLine(standardLine);
            for (int j = 0; j < _sideLength; j++)
            {
                layout.Append("|  ");
                layout.Append(CellSymbol(j, i));
                layout.Append("  ");
            }

            layout.AppendLine("|");
        }

        layout.Append($"{standardLine}\n");

        return layout.ToString();
    }

    /// <summary>
    /// Clears all cells in the grid.
    /// </summary>
    internal void ClearCells()
    {
        for (int i = 0; i < _sideLength; i++)
        {
            for (int j = 0; j < _sideLength; j++)
            {
                _gridFormat[i, j] = null;
            }
        }
    }

    /// <summary>
    /// Retrieves the symbol at the specified position in the grid.
    /// If the cell is empty, a space character is returned.
    /// </summary>
    /// <param name="index1">The row index of the cell.</param>
    /// <param name="index2">The column index of the cell.</param>
    /// <returns>The symbol at the specified position, or a space character if the cell is empty.</returns>
    private char? CellSymbol(int index1, int index2)
    {
        return _gridFormat[index1, index2] ?? ' ';
    }

    /// <summary>
    /// Determines whether the player has won vertically or horizontally.
    /// </summary>
    /// <param name="symbolToCheckFor">The symbol to check for.</param>
    /// <returns><c>true</c> if the player has won vertically or horizontally; otherwise, <c>false</c>.</returns>
    private bool PlayerHasWonVerticallyOrHorizontally(char symbolToCheckFor)
    {
        // Check rows and columns
        for (int i = 0; i < _sideLength; i++)
        {
            bool rowMatch = true;
            bool colMatch = true;

            for (int j = 0; j < _sideLength; j++)
            {
                // Check horizontally
                if (_gridFormat[i, j] != symbolToCheckFor)
                {
                    rowMatch = false;
                }

                // Check vertically
                if (_gridFormat[j, i] != symbolToCheckFor)
                {
                    colMatch = false;
                }

                // Early exit if both row and column don't match
                if (!rowMatch && !colMatch)
                {
                    break;
                }
            }

            if (rowMatch || colMatch)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the player has won diagonally.
    /// </summary>
    /// <param name="symbolToCheckFor">The symbol to check for.</param>
    /// <returns><c>true</c> if the player has won diagonally; otherwise, <c>false</c>.</returns>
    private bool PlayerHasWonDiagonally(char symbolToCheckFor)
    {
        bool leftToRightMatch = true;
        bool rightToLeftMatch = true;

        for (int i = 0; i < _sideLength; i++)
        {
            // Check Left-to-Right diagonal
            if (_gridFormat[i, i] != symbolToCheckFor)
            {
                leftToRightMatch = false;
            }

            // Check Right-to-Left diagonal
            if (_gridFormat[_sideLength - i - 1, i] != symbolToCheckFor)
            {
                rightToLeftMatch = false;
            }

            // Early exit if both diagonals don't match
            if (!leftToRightMatch && !rightToLeftMatch)
            {
                break;
            }
        }

        return leftToRightMatch || rightToLeftMatch;
    }
}