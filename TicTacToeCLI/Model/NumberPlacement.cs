using Microsoft.VisualBasic.CompilerServices;
using TicTacToeCLI.Model;

namespace TicTacToeCLI.Models;

public class NumberPlacement
{
    private static int _gridSize;
    private readonly int _number;

    public NumberPlacement(int number)
    {
        _number = number;
    }

    public static explicit operator IntegerPair(NumberPlacement number)
    {
        int in1 = number._number % _gridSize;
        int in2 = number._number / _gridSize;

        return new IntegerPair(in1, in2);
    }

    public static implicit operator NumberPlacement(IntegerPair pair)
    {
        int first = pair.First;
        int second = pair.Second * _gridSize;

        return new NumberPlacement((first + second));

        // 0  1  2

        // 0 = 0, 3, 6
        // 1 = 1, 4, 7
        // 2 = 2, 5, 8
    }

    public override string ToString()
    {
        return $"{_number + 1}";
    }

    public static void SetGridSize(int gridSize)
    {
        _gridSize = gridSize;
    }
}