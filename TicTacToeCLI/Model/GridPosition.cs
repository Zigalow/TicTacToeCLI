namespace TicTacToeCLI.Model;

/// <summary>
/// Represents a position on the Tic-Tac-Toe grid using a single number.
/// This struct allows for conversion between single-number representation and coordinate pairs.
/// </summary>
/// <remarks>
/// Every calculation is done using 0-based index.
/// </remarks>
public readonly struct GridPosition
{
    /// <summary>
    /// Represents the position of top-left corner of the game grid.
    /// </summary>
    internal static readonly IntegerPair TopLeft = new(0, 0);

    /// <summary>
    /// Represents the position of top-right corner of the game grid.
    /// </summary>
    internal static readonly IntegerPair TopRight = new(Game.GameGridSideLength - 1, 0);

    /// <summary>
    /// Represents the position of the middle cell of the game grid.
    /// </summary>
    internal static readonly IntegerPair Middle = new(Game.GameGridSideLength / 2, Game.GameGridSideLength / 2);

    /// <summary>
    /// Represents the position of the bottom-left corner of the game grid.
    /// </summary>
    internal static readonly IntegerPair BottomLeft = new(0, Game.GameGridSideLength - 1);

    /// <summary>
    /// Represents the position of the bottom-right corner of the game grid.
    /// </summary>
    internal static readonly IntegerPair BottomRight = new(Game.GameGridSideLength - 1, Game.GameGridSideLength - 1);

    private const int GridSize = Game.GameGridSideLength;

    /// <summary>
    /// Gets the linear index representation of the grid position (0-based index).
    /// </summary>
    private int LinearIndex { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridPosition"/> class.
    /// </summary>
    /// <param name="linearIndex">The number representing the position on the grid (0-based index).</param>
    /// <example>
    /// LinearIndex = 0 - would represent the upper left corner of the grid (0,0).<br/>
    /// LinearIndex = 2 - would represent the upper right corner of the grid (0,2).<br/>
    /// LinearIndex = 6 - would represent the lower left corner of the grid (2,0).<br/>
    /// LinearIndex = 8 - would represent the lower right corner of the grid (2,2).<br/>
    /// </example>
    public GridPosition(int linearIndex)
    {
        LinearIndex = linearIndex;
    }

    /// <summary>
    /// Explicitly converts a <see cref="GridPosition"/> to an <see cref="IntegerPair"/>.
    /// </summary>
    /// <param name="gridPosition">The <see cref="GridPosition"/> to convert.</param>
    /// <returns>An <see cref="IntegerPair"/> representing the grid coordinates (column, row), (0-based index).</returns>
    /// <remarks>
    /// The conversion is done using the formula:<br/>
    /// column = number % GridSize <br/>
    /// row = number / GridSize<br/>
    /// </remarks>
    public static explicit operator IntegerPair(GridPosition gridPosition)
    {
        int columnNumber = gridPosition.LinearIndex % GridSize;
        int rowNumber = gridPosition.LinearIndex / GridSize;

        return new IntegerPair(columnNumber, rowNumber);
    }

    /// <summary>
    /// Implicitly converts an <see cref="IntegerPair"/> to a <see cref="GridPosition"/>.
    /// </summary>
    /// <param name="pair">The <see cref="IntegerPair"/> to convert.</param>
    /// <returns>A <see cref="GridPosition"/> representing the position as a single number (0-based index).</returns>
    /// <remarks>
    /// The conversion is done using the formula:<br/>
    /// number = column + (row * GridSize)<br/>
    /// </remarks>
    public static implicit operator GridPosition(IntegerPair pair)
    {
        int column = pair.First;
        int rowTimesGridSize = pair.Second * GridSize;

        return new GridPosition(column + rowTimesGridSize);
    }

    /// <summary>
    /// Returns a string representation of the position.
    /// </summary>
    /// <returns>A string representing the position (1-based index).</returns>
    /// <remarks>This is what the user sees and uses, since the grid in the TUI is 1-based index.</remarks>
    public override string ToString()
    {
        return $"{LinearIndex + 1}";
    }
}