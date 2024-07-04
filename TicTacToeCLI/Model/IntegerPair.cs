namespace TicTacToeCLI.Model;

/// <summary>
/// Pair of integers - corresponds to the grid. 
/// </summary>
/// <example>
/// Integer pair of [0,0] would be upper left
/// <code>
/// IntegerPair pair = new IntegerPair(0,0);
/// </code>
/// </example>
/// 
public class IntegerPair(int first, int second)
{
    /// <summary>
    /// First value of <see cref="IntegerPair"/>
    /// </summary>
    public int First { get; } = first;

    /// <summary>
    /// Second value of <see cref="IntegerPair"/>
    /// </summary>
    public int Second { get; } = second;

    /// <summary>
    /// Returns a string that represents the <see cref="IntegerPair"/>.
    /// </summary>
    /// <returns>Returns the <see cref="IntegerPair"/> from the user's point of view. </returns>
    ///<example>
    /// An <see cref="IntegerPair"/> of [0,0] would return the string; [1,1], which is the upper left corner of the grid
    /// </example>
    /// 
    public override string ToString()
    {
        return $"{First + 1}.{Second + 1}";
    }
}