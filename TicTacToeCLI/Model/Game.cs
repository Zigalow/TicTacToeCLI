using TicTacToeCLI.Models;
using TicTacToeCLI.View;

namespace TicTacToeCLI.Model;

public class Game
{
    public int TurnCounter { get; private set; } = 1;

    protected readonly Player[] Players = new Player[2];
    private readonly Grid _gameGrid;

    protected readonly int GridSideLength = 3;


    public int GameGridSideLength
    {
        get => GridSideLength;
    }

    public Grid GameGrid
    {
        get => _gameGrid;
        private init => _gameGrid = value;
    }


    public Player CurrentPlayer { get; private set; }

    public Game(Player player1, Player cpu)
    {
        GameGrid = new Grid(GridSideLength);
        Players[0] = player1;
        Players[1] = cpu;
        CurrentPlayer = Players[0];
        NumberPlacement.SetGridSize(GameGridSideLength);
    }


    public bool CurrentPlayerHasWon()
    {
        int matchesSideways = 0;
        int matchesVertically = 0;
        int matchesLeftCrosses = 0;
        int matchesRightCrosses = 0;
        char symbolToCheckFor = CurrentPlayer.Symbol;
        for (int i = 0; i < GridSideLength; i++)
        {
            for (int j = 0; j < GridSideLength; j++)
            {
                // Vertically and Sideways
                if (symbolToCheckFor == GameGrid[i, j] || symbolToCheckFor == GameGrid[j, i])
                {
                    // Sideways
                    if (symbolToCheckFor == GameGrid[j, i])
                    {
                        matchesSideways++;
                    }

                    // Vertically
                    if (symbolToCheckFor == GameGrid[i, j])
                    {
                        matchesVertically++;
                    }
                }
                else
                {
                    break;
                }
            }

            if (matchesSideways == GridSideLength || matchesVertically == GridSideLength)
            {
                return true;
            }

            matchesVertically = 0;
            matchesSideways = 0;
        }

        // Crosses

        for (int i = 0; i < GridSideLength; i++)
        {
            // Left_to_Right cross
            if (symbolToCheckFor == GameGrid[i, i])
            {
                matchesLeftCrosses++;
            }

            // Right_to_Left cross
            if (symbolToCheckFor == GameGrid[GridSideLength - i - 1, i])
            {
                matchesRightCrosses++;
            }
        }

        return GridSideLength == matchesLeftCrosses || GridSideLength == matchesRightCrosses;
    }

    public bool AllGridsFilled()
    {
        foreach (var element in GameGrid)
        {
            if (element == null) return false;
        }

        return true;
    }

    public void NextPlayer()
    {
        CurrentPlayer = CurrentPlayer == Players[0] ? Players[1] : Players[0];
    }

    public void ChooseRandomPlayer()
    {
        CurrentPlayer = new Random().Next(2) == 0 ? Players[0] : Players[1];
    }

    public void IncreaseTurnCounter()
    {
        TurnCounter++;
    }

    public void ResetGameData()
    {
        foreach (var player in Players)
        {
            player.ResetData();
        }

        TurnCounter = 1;
    }
}