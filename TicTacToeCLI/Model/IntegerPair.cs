namespace TicTacToeCLI.Model;

/// <summary>
/// Pair of integers - corresponds to the grid. Integer pair of [0,0] would be upper left
/// </summary>
public class IntegerPair(int first, int second)
{
    // private readonly int _first;
    // private readonly int _second;

    /// <summary>
    /// First value of integer pair
    /// </summary>
    public int First { get; } = first;

    /// <summary>
    /// Second value of integer pair
    /// </summary>
    public int Second { get; } = second;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{First + 1}.{Second + 1}";
    }
}