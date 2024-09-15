namespace TicTacToeCLI.Model;


/// <summary>
/// Represents a pair of integers corresponding to coordinates on a grid.
/// </summary>
/// <remarks>
/// The coordinates are zero-based internally, but displayed as one-based to the user.
/// </remarks>
/// <example>
/// Creating a pair representing the upper-left corner of the grid:
/// <code>
/// GridCoordinate pair = new GridCoordinate(0, 0);
/// </code>
/// </example>
public readonly struct GridCoordinate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GridCoordinate"/> class.
    /// </summary>
    /// <param name="first">The first coordinate (column) of the pair.</param>
    /// <param name="second">The second coordinate (row) of the pair.</param>
    public GridCoordinate(int first, int second)
    {
        First = first;
        Second = second;
    }

    /// <summary>
    /// Gets the first value (column) of the <see cref="GridCoordinate"/>.
    /// </summary>
    /// <remarks>This value is zero-based internally.</remarks>
    public int First { get; }

    /// <summary>
    /// Gets the first value (row) of the <see cref="GridCoordinate"/>.
    /// </summary>
    /// <remarks>This value is zero-based internally.</remarks>
    public int Second { get; }

    /// <summary>
    /// Returns a string that represents the <see cref="GridCoordinate"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="GridCoordinate"/> from the user's point of view.</returns>
    /// <remarks>
    /// The returned string uses 1-based indexing for user-friendly display.
    /// </remarks>
    /// <example>
    /// A <see cref="GridCoordinate"/> of [0,0] would return the string "1.1", 
    /// which represents the upper-left corner of the grid from the user's perspective.
    /// </example>
    public override string ToString()
    {
        return $"{First + 1}.{Second + 1}";
    }
}