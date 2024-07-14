using System.Collections;
using System.Text;

namespace TicTacToeCLI.Model;

public class Grid : IEnumerable
{
    private readonly char?[,] GridFormat;
    private readonly int SideLength;

    public Grid(int size)
    {
        if (size % 2 == 0)
        {
            throw new EvenGridSizeException();
        }

        GridFormat = new char?[size, size];
        SideLength = size;
    }

    public char? this[int index1, int index2]
    {
        get => GridFormat[index1, index2];
        set => GridFormat[index1, index2] = value;
    }

    public char? this[IntegerPair pair]
    {
        get => GridFormat[pair.First, pair.Second];
        set => GridFormat[pair.First, pair.Second] = value;
    }

    public IEnumerator GetEnumerator()
    {
        return GridFormat.GetEnumerator();
    }

    public override string ToString()
    {
        StringBuilder layout = new();

        string standardLine = "+" + new StringBuilder(4 * SideLength).Insert(0, "-----+", SideLength);

        for (int i = 0; i < SideLength; i++)
        {
            layout.AppendLine(standardLine);
            for (int j = 0; j < SideLength; j++)
            {
                layout.Append("|  ");
                layout.Append(InsertSymbol(j, i));
                layout.Append("  ");
            }

            layout.AppendLine("|");
        }

        layout.Append($"{standardLine}\n");

        return layout.ToString();
    }

    private char? InsertSymbol(int index1, int index2)
    {
        if (GridFormat[index1, index2] == null)
        {
            return ' ';
        }

        return GridFormat[index1, index2];
    }

    public bool IsFull()
    {
        foreach (var cell in GridFormat)
        {
            if (cell == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool HasPlayerWon(Player player)
    {
        int matchesSideways = 0;
        int matchesVertically = 0;
        int matchesLeftCrosses = 0;
        int matchesRightCrosses = 0;
        char symbolToCheckFor = player.Symbol;
        for (int i = 0; i < SideLength; i++)
        {
            for (int j = 0; j < SideLength; j++)
            {
                // Vertically and Sideways
                if (symbolToCheckFor == GridFormat[i, j] || symbolToCheckFor == GridFormat[j, i])
                {
                    // Sideways
                    if (symbolToCheckFor == GridFormat[j, i])
                    {
                        matchesSideways++;
                    }

                    // Vertically
                    if (symbolToCheckFor == GridFormat[i, j])
                    {
                        matchesVertically++;
                    }
                }
                else
                {
                    break;
                }
            }

            if (matchesSideways == SideLength || matchesVertically == SideLength)
            {
                return true;
            }

            matchesVertically = 0;
            matchesSideways = 0;
        }

        // Crosses

        for (int i = 0; i < SideLength; i++)
        {
            // Left_to_Right cross
            if (symbolToCheckFor == GridFormat[i, i])
            {
                matchesLeftCrosses++;
            }

            // Right_to_Left cross
            if (symbolToCheckFor == GridFormat[SideLength - i - 1, i])
            {
                matchesRightCrosses++;
            }
        }

        return SideLength == matchesLeftCrosses || SideLength == matchesRightCrosses;
    }

    public bool UnavailableSpace(int in1, int in2)
    {
        return GridFormat[in1, in2] == null;
    }

    internal void ClearCells()
    {
        for (int i = 0; i < SideLength; i++)
        {
            for (int j = 0; j < SideLength; j++)
            {
                GridFormat[i, j] = null;
            }
        }
    }
}