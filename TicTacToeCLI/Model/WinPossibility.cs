using System.Collections.Immutable;

namespace TicTacToeCLI.Model;

/// <summary>
/// Class containing lists of different win-possibilities. Empty lists means no win-possibility.
/// Doesn't take into account if opponent is blocking a win-opportunity.
/// </summary>
public class WinPossibility
{
    /// <summary>
    /// ImmutableList holding numbers that corresponds to a win-possibility on the x-axis
    /// </summary>
    public ImmutableList<int> Sides { get; }

    /// <summary>
    /// ImmutableList holding numbers that corresponds to a win-possibility on the y-axis
    /// </summary>
    public ImmutableList<int> Vertically { get; }

    /// <summary>
    /// ImmutableArray holding two boolean values. First value indicates if there is a win-possibility from the cross,
    /// spanning from left to right. The second value indicates if there is a win-possibility spanning from right to left.
    /// </summary>
    public ImmutableArray<bool> Crosses { get; }

    /// <summary>
    /// Constructor for extracting different win-possibilities. The constructor doesn't take into account that an opposition player,
    /// might be blocking a win-opportunity. Empty lists means no win-possibility.
    /// </summary>
    /// <param name="currentlyPlacedSpaces">List of indices corresponding to places where the player has placed
    /// its symbols on the grid</param>
    /// <param name="gridLength">Side length of the grid that's currently being played on.</param>
    public WinPossibility(IReadOnlyList<GridCoordinate> currentlyPlacedSpaces, int gridLength)
    {
        var vert =
            currentlyPlacedSpaces.GroupBy(pair => pair.First)
                .Where(group => group.Count() >= gridLength - 1)
                .Select(group => group.Key).ToImmutableList();

        var sides = currentlyPlacedSpaces.GroupBy(pair => pair.Second)
            .Where(group => group.Count() >= gridLength - 1)
            .Select(group => group.Key).ToImmutableList();

        int matchesLeftCrosses = 0;
        int matchesRightCrosses = 0;

        foreach (var pair in currentlyPlacedSpaces)
        {
            if (pair.First == pair.Second)
            {
                matchesLeftCrosses++;
            }

            if (pair.First + pair.Second == gridLength - 1)
            {
                matchesRightCrosses++;
            }
        }

        bool[] crosses =
        [
            matchesLeftCrosses == gridLength - 1,
            matchesRightCrosses == gridLength - 1
        ];

        Sides = sides;
        Vertically = vert;
        Crosses = [..crosses];
    }
}