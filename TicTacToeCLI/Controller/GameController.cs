using System.Diagnostics;
using TicTacToeCLI.Model;

// Todo - XML-doc and code refactoring (code analysis) - [in progress]
// Todo - Enhance UI experience in terms of text - especially when it comes to the rules or when mistyping
// Todo - Add readme
// Todo - Auto fills the spots when it's a tie (maybe)
namespace TicTacToeCLI.Controller;

public class GameController
{
    private bool _hasShownRules = false;
    private const int DelayInMicroseconds = 15000;
    private Game CurrentGame { get; set; }
    private GameMode _gameMode;

    public void Start()
    {
        WelcomeMessage();
        bool sameConfig = false;
        bool exit;

        do
        {
            if (!sameConfig)
            {
                Setup();
            }

            RunGame(out exit, out sameConfig);
        } while (!exit);
    }

    private void Setup()
    {
        _gameMode = SelectGameMode();
        if (SkipShapeSelection())
        {
            CurrentGame = _gameMode == GameMode.PlayerVersusPlayer
                ? new Game(new Player('X'), new Player('O'))
                : new CpuGame(new Player('X'), new Cpu('O'));
            return;
        }

        (Player player1, Player player2) = SelectShapes(_gameMode);

        CurrentGame = _gameMode switch
        {
            GameMode.PlayerVersusPlayer => new Game(player1, player2),
            GameMode.PlayerVersusCpu when player2 is Cpu cpu => new CpuGame(player1, cpu),
            _ => throw new InvalidOperationException("Invalid game mode or player configuration")
        };
    }

    private void ExplainRules()
    {
        SlowPrint("Explaining the rules:");
        Grid ruleGrid = new Grid(3)
        {
            [0, 0] = 'X',
            [0, 1] = 'A',
            [1, 0] = 'B',
        };
        Console.WriteLine(ruleGrid);
        Thread.Sleep(1000);
        SlowPrint(
            "Placing a symbol needs to be written in the format of \"x,y\" or \"x.y\", where x is the x-axis, and y is the y-axis." +
            "\nFor example, the top left corner, X, is 1,1, whereas the space below that, A, would be 1,2. The space to right of the top left corner, B, is 2,1." +
            "\nSpecify where you want to place your symbol and press enter.", 50000);

        Console.WriteLine("Press a button start the game...");
        Console.ReadLine();
    }

    private void NextTurn()
    {
        CurrentGame.NextPlayer();
        CurrentGame.IncreaseTurnCounter();
    }

    private void PerformMove()
    {
        if (CurrentGame.CurrentPlayer is Cpu)
        {
            PerformCPUMove();
        }
        else
        {
            PerformPlayerMove();
        }
    }

    private void GameFinishedChoiceDialog(out bool exitGame, out bool playAgainWithSameConfigs)
    {
        exitGame = false;
        playAgainWithSameConfigs = false;

        SlowPrint("\nWould you like to play again?\n");

        Console.WriteLine("Play again with same configurations - (Press 1)");
        Console.WriteLine("Play again with different configurations - (Press 2)");
        Console.WriteLine("Exit game - (Press 3)\n");

        ConsoleKey playAgain;
        do
        {
            playAgain = ReadInputKey();
        } while (playAgain != ConsoleKey.D1 && playAgain != ConsoleKey.D2 && playAgain != ConsoleKey.D3);

        switch (playAgain)
        {
            case ConsoleKey.D1:
                CurrentGame.ResetGame();
                playAgainWithSameConfigs = true;
                break;
            case ConsoleKey.D2:
                playAgainWithSameConfigs = false;
                break;
            default:
                exitGame = true;
                break;
        }
    }

    private void RunGame(out bool exitGameChoice, out bool playAgainWithSameConfigsChoice)
    {
        CommencingGameMessage();

        exitGameChoice = false;
        playAgainWithSameConfigsChoice = false;

        MainGameLoop();

        Thread.Sleep(2500);

        // CurrentGame.ResetPlayerData();

        GameFinishedChoiceDialog(out exitGameChoice, out playAgainWithSameConfigsChoice);
    }

    private void DefaultCurrentPlayerTurnMessage()
    {
        Console.WriteLine();
        SlowPrint($"{CurrentGame.CurrentPlayer} has the current turn:\n");
    }

    private void DisplayRules()
    {
        SlowPrint(
            "Specify your move, in one of the two formats below and press enter:");
        Thread.Sleep(500);
        Console.WriteLine(
            $"\"x.y\" or \"x,y\": x is the x-axis, and y is the y-axis. Top left corner is 1.1, whereas bottom left corner would be \"1.{Game.GameGridSideLength}\".");
        Console.WriteLine(
            $"\"z\": z is the allocated space in the grid. Top left corner would be 1, whereas bottom left corner would be {Game.GameGridSideLength * Game.GameGridSideLength - (Game.GameGridSideLength - 1)}.");
        // Thread.Sleep(1000);
        // SlowPrint("...", 500000);
        Thread.Sleep(1000);
    }

    private void PlayerPlacedSymbolMessage(Player player, IntegerPair pair)
    {
        SlowPrint($"{player} placed a symbol on {(GridPosition)pair} / {pair}");
    }

    private void DefaultWrongFormatMessage(string text)
    {
        SlowPrint(text);
        Thread.Sleep(500);
        SlowPrint("Try again...");
        Thread.Sleep(1500);
        Console.WriteLine(CurrentGame.GameGrid);
        DisplayRules();
    }

    private void PerformCPUMove()
    {
        var cpu = CurrentGame.CurrentPlayer as Cpu;
        var cpuGame = CurrentGame as CpuGame;
        IntegerPair pairToUse;

        bool getRandomMove = cpuGame.CpuCanWin(out var returnedPair) ? false : !cpuGame.CpuCanLose(out returnedPair);

        if (getRandomMove)
        {
            do
            {
                pairToUse = cpu.GetRandomPosition();
            } while (!ValidMove(pairToUse));
        }
        else
        {
            Debug.Assert(returnedPair != null, nameof(returnedPair) + " != null");
            pairToUse = returnedPair.Value;
        }

        cpu.AddSymbolPosition(pairToUse);
        CurrentGame.GameGrid[pairToUse.First, pairToUse.Second] = CurrentGame.CurrentPlayer.Symbol;
        Console.WriteLine(CurrentGame.GameGrid);
        PlayerPlacedSymbolMessage(CurrentGame.CurrentPlayer, pairToUse);
        Thread.Sleep(1000);
    }

    private void PerformPlayerMove()
    {
        // TODO - Maybe remove loop
        while (true)
        {
            if (!_hasShownRules)
            {
                if (CurrentGame.TurnCounter is 1 or 2)
                {
                    if (CurrentGame.TurnCounter is 1)
                    {
                        DisplayRules();
                        DefaultCurrentPlayerTurnMessage();
                    }
                    else
                    {
                        DefaultCurrentPlayerTurnMessage();
                        DisplayRules();
                    }

                    _hasShownRules = true;
                }
            }
            else
            {
                DefaultCurrentPlayerTurnMessage();
            }

            var input = ReadInput();
            Console.In.Close();

            string[] splitByComma = input.Split(",");
            string[] splitByDot = input.Split(".");

            string[]? parts = splitByComma.Length == 2 ? splitByComma :
                splitByDot.Length == 2 ? splitByDot : null;

            IntegerPair pair;
            try
            {
                if (parts != null) // If it isn't null, the Integer pair will have length of 2
                {
                    int in1 = Convert.ToInt32(parts[0]) - 1;
                    int in2 = Convert.ToInt32(parts[1]) - 1;
                    pair = new IntegerPair(in1, in2);
                    if (pair.First < 0 || 0 > pair.Second)
                    {
                        throw new Exception();
                    }
                }
                else if (char.IsNumber(input.ToCharArray()[0]))
                {
                    GridPosition number =
                        new GridPosition(Convert.ToInt32(input) - 1);
                    pair = (IntegerPair)number;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                DefaultWrongFormatMessage("The move was written in an incorrect format.");
                continue;
            }

            if (pair.First >= Game.GameGridSideLength || pair.Second >= Game.GameGridSideLength)
            {
                DefaultWrongFormatMessage("You are trying to place a symbol outside of the grid.");
                continue;
            }

            if (!ValidMove(pair))
            {
                SlowPrint("The space is already being occupied.");
                SlowPrint("Choose another space.");
                Console.WriteLine(CurrentGame.GameGrid);
            }
            else
            {
                CurrentGame.CurrentPlayer.AddSymbolPosition(pair);
                CurrentGame.GameGrid[pair] = CurrentGame.CurrentPlayer.Symbol;
                Console.WriteLine(CurrentGame.GameGrid);
                PlayerPlacedSymbolMessage(CurrentGame.CurrentPlayer, pair);
                Thread.Sleep(1000);
                return;
            }
        }
    }

    private void WelcomeMessage()
    {
        if (Console.KeyAvailable)
        {
        } // Prevents the user from typing while welcome message is being printed

        SlowPrint("Welcome to TicTacToe", 3000);
        Thread.Sleep(2000);
        SlowPrint("I hope you enjoy the experience\n\n", 3000);
        Thread.Sleep(1000);
    }

    private GameMode SelectGameMode()
    {
        SlowPrint("How would you like to play?");

        Console.WriteLine("Alone versus a CPU - (Press 1)");
        Console.WriteLine("Versus another local player - (Press 2)\n");

        ConsoleKey chosenKey;
        do
        {
            chosenKey = ReadInputKey();
        } while (chosenKey != ConsoleKey.D1 && chosenKey != ConsoleKey.D2);

        GameMode chosenGameMode = chosenKey == ConsoleKey.D1 ? GameMode.PlayerVersusCpu : GameMode.PlayerVersusPlayer;

        string modeDescription = chosenGameMode == GameMode.PlayerVersusCpu ? "a CPU" : "a local player";

        SlowPrint($"\nYou have chosen to play versus {modeDescription}.\n\n");

        return chosenGameMode;
    }

    private (Player player1, Player player2) SelectShapes(GameMode gameMode)
    {
        char GetShapeInput(string prompt, char? excludeShape = null)
        {
            SlowPrint(prompt);
            SlowPrint("Press the button with the corresponding letter, from the alphabet, to choose your shape.");

            ConsoleKeyInfo keyPressed;
            char shape;
            do
            {
                keyPressed = ReadInputKeyInfo();
                shape = char.ToUpper(keyPressed.KeyChar);
            } while (!char.IsLetter(shape) || shape == excludeShape);

            Console.WriteLine();
            return shape;
        }

        string player1Prompt = gameMode == GameMode.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 1?"
            : "Which shape would you like for yourself to be?";

        char shape1 = GetShapeInput(player1Prompt);

        SlowPrint(gameMode == GameMode.PlayerVersusPlayer
            ? $"Player 1 chose the shape {shape1}."
            : $"You chose the shape {shape1} for yourself.");

        Console.WriteLine();

        string player2Prompt = gameMode == GameMode.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 2?"
            : "Which shape would you like for the CPU to be?";

        char shape2 = GetShapeInput(player2Prompt, shape1);

        SlowPrint(gameMode == GameMode.PlayerVersusPlayer
            ? $"Player 2 chose the shape {shape2}"
            : $"You chose the shape {shape2} for the CPU");

        Console.WriteLine();

        return (new Player(shape1), gameMode == GameMode.PlayerVersusPlayer ? new Player(shape2) : new Cpu(shape2));
    }

    private bool ValidMove(int in1, int in2)
    {
        return CurrentGame.GameGrid.UnavailableSpace(in1, in2);
    }

    private bool ValidMove(IntegerPair pair)
    {
        return CurrentGame.GameGrid.UnavailableSpace(pair.First, pair.Second);
    }

    private bool SkipShapeSelection()
    {
        SlowPrint("Do you wish to select your shapes?");

        Console.WriteLine("No - (Press 1)");
        Console.WriteLine("Yes - (Press 2)\n");
        ConsoleKey selectShapeButton;
        do
        {
            selectShapeButton = ReadInputKey();
        } while (selectShapeButton != ConsoleKey.D1 && selectShapeButton != ConsoleKey.D2);

        return selectShapeButton == ConsoleKey.D1;
    }

    private void SlowPrint(String text, int delayInMicroseconds = DelayInMicroseconds, bool newLine = true)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(TimeSpan.FromMicroseconds(delayInMicroseconds));
        }

        if (newLine)
        {
            Console.WriteLine();
        }

        Thread.Sleep(700);
    }

    private void CommencingGameMessage()
    {
        SlowPrint("The game will now commence...");
        Thread.Sleep(1000);
        // CurrentGame.ChooseRandomPlayer();
        Console.WriteLine();
        Console.WriteLine(CurrentGame.GameGrid);
        SlowPrint($"{CurrentGame.CurrentPlayer} will start the turn...\n");
        // SlowPrint("...", delayInMicroseconds: 333000, false);
        // SlowPrint("\n");
        Thread.Sleep(2000);
    }

    private void MainGameLoop()
    {
        while (true)
        {
            PerformMove();

            if (CurrentGame.CurrentPlayerHasWon())
            {
                SlowPrint($"\nCongratulations to {CurrentGame.CurrentPlayer} on winning the game...");
                return;
            }

            if (CurrentGame.GameIsDrawn())
            {
                SlowPrint("There are no available spaces left, and the game has ended in a tie...");
                return;
            }

            NextTurn();
        }
    }

    private string? ReadInput()
    {
        while (Console.KeyAvailable) // Clear the input buffer
        {
            Console.ReadKey(true);
        }

        return Console.ReadLine();
    }

    private ConsoleKey ReadInputKey()
    {
        while (Console.KeyAvailable) // Clear the input buffer
        {
            Console.ReadKey(true);
        }

        return Console.ReadKey(true).Key;
    }

    private ConsoleKeyInfo ReadInputKeyInfo()
    {
        while (Console.KeyAvailable) // Clear the input buffer
        {
            Console.ReadKey(true);
        }

        return Console.ReadKey(true);
    }
}