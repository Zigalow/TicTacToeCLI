using System.Collections;
using System.Text;
using TicTacToeCLI.Model;

namespace TicTacToeCLI.View;

public class Grid : IEnumerable
{
    private readonly char?[,] GridFormat;
    private readonly int SideLength; 

    public Grid(int size)
    {
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


    public bool UnavailableSpace(int in1, int in2)
    {
        return GridFormat[in1, in2] == null;
    }

    public void EmptyGrid()
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