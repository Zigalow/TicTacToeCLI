namespace TicTacToeCLI.Models;

public class CPUGame : Game
{
    private CPU CPU { get; }
    // private CPU _cpu;

    public CPUGame(Player player1, CPU player2) : base(player1, player2)
    {
        player2.FillAvailableSpaces(GridSideLength);
        CPU = player2;
    }

    public bool CPUCanWin(out IntegerPair? winningPair)
    {
        WinPossibility winPossibility = new(CPU.PlacedCurrently, GridSideLength);

        return CalculateWinPossibility(winPossibility, out winningPair);
    }

    public bool CPUCanLose(out IntegerPair? pairToPreventLoss)
    {
        WinPossibility winPossibility = new(Players[0].PlacedCurrently, GridSideLength);

        return CalculateWinPossibility(winPossibility, out pairToPreventLoss);
    }


    private bool CalculateWinPossibility(WinPossibility winPossibility, out IntegerPair? pair)
    {
        // Sideways
        foreach (var sides in winPossibility.Sides)
        {
            for (int i = 0; i < GridSideLength; i++)
            {
                if (GameGrid[i, sides] == null)
                {
                    pair = new IntegerPair(i, sides);
                    return true;
                }
            }
        }

        // Vertically
        foreach (var vert in winPossibility.Vertically)
        {
            for (int i = 0; i < GridSideLength; i++)
            {
                if (GameGrid[vert, i] == null)
                {
                    pair = new IntegerPair(vert, i);
                    return true;
                }
            }
        }

        // Crosses

        //Left_to_Right then Right_to_Left crosses
        for (var i = 0; i < 2; i++)
        {
            // First index is true, if a win-possibility is present for left_to_right cross. Second index is for right_to_left cross
            if (!winPossibility.Crosses[i])
            {
                continue;
            }

            if (i == 0) // If there is a win-possibility with the left_to_right cross
            {
                for (var j = 0; j < GridSideLength; j++)
                {
                    if (GameGrid[j, j] != null)
                    {
                        continue;
                    }
                    
                    pair = new IntegerPair(j, j);
                    return true;
                }
            }
            else // There is a win-possibility with the right_to_left cross
            {
                for (var j = 0; j < GridSideLength; j++)
                {
                    if (GameGrid[GridSideLength - j - 1, j] != null)
                    {
                        continue;
                    }
                    pair = new IntegerPair(GridSideLength - j - 1, j);
                    return true;
                }
            }
        }

        pair = null;
        return false;
    }
}