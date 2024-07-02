using System.Collections;
using Microsoft.VisualBasic.CompilerServices;
using TicTacToeCLI.Model;
using TicTacToeCLI.View;

namespace TicTacToeCLI.Models;

public class CPU : Player
{
    private List<IntegerPair> _availablePairs;

    private List<IntegerPair> DefaultAvailableSpaces { get; set; }

    public CPU(char symbol) : base(symbol)
    {
        Name = "CPU";
    }

    public void FillAvailableSpaces(int gridSideLength)
    {
        DefaultAvailableSpaces = new List<IntegerPair>();

        for (int i = 0; i < gridSideLength; i++)
        {
            for (int j = 0; j < gridSideLength; j++)
            {
                DefaultAvailableSpaces.Add(new IntegerPair(i, j));
            }
        }
        _availablePairs = new List<IntegerPair>(DefaultAvailableSpaces);
    }

    /*public void GetRandomSpace(out int in1, out int in2)
    {
        int index = new Random().Next(_availablePairs.Count);
        IntegerPair pair = _availablePairs[index];
        _availablePairs.RemoveAt(index);

        in1 = pair.First;
        in2 = pair.Second;
    }*/

    public IntegerPair GetRandomSpace()
    {
        int index = new Random().Next(_availablePairs.Count);
        IntegerPair pair = _availablePairs[index];
        _availablePairs.RemoveAt(index);
       
        
        return pair;
    }

    public override void ResetData()
    {
        _availablePairs = new List<IntegerPair>(DefaultAvailableSpaces);
        PlacedCurrently.Clear();
    }
}