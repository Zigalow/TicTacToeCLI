namespace TicTacToeCLI.Model;

/// <summary>
/// This class represents a cpu.
/// Besides inheriting from the <see cref="Player"/> class,
/// it contains a list of positions on the grid that are registered as free,
/// as well as methods for getting random positions and its own method of clearing its data   
/// </summary>
/// <constructor>
/// Constructor for cpu where name is given as 'CPU', and the symbol is given as parameter
/// </constructor>
public class Cpu(char symbol) : Player("CPU", symbol)
{
    private static readonly Random Random = new();
    private List<IntegerPair> _availablePairs = new(CpuGame.AllGridPositions);

    /// <summary>
    /// Returns a random <see cref="IntegerPair"/>. This is used when the cpu has to generate a random move.
    /// The move will be generated from the free positions that the cpu has registered in a list.
    /// After a random move has been generated, it will remove that move the list of registered positions that are free
    /// </summary>
    /// <returns>a randomly generated move from a list of free positions</returns>
    public IntegerPair GetRandomPosition()
    {
        int index = Random.Next(_availablePairs.Count);
        IntegerPair pair = _availablePairs[index];
        _availablePairs.RemoveAt(index);

        return pair;
    }

    /// <summary>
    /// Clears the data of the CPU. This includes clearing the list of <see cref="Player.SymbolPositions"/>
    /// </summary>
    internal override void ClearData()
    {
        base.ClearData();
        _availablePairs = new List<IntegerPair>(CpuGame.AllGridPositions);
    }
}