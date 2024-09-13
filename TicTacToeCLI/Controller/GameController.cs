using TicTacToeCLI.Model;
using TicTacToeCLI.Model.GameOptions;
using static System.Int32;

// Todo - XML-doc and code refactoring (code analysis) - [in progress]
// Todo - Add readme

// Todo - Auto fills the spots when it's a tie (maybe)
// Todo - Setup configurations in github, so people can't accept their own pull request
namespace TicTacToeCLI.Controller;

public class GameController
{
    // private bool _hasShownRules = false;
    private const int TextDelayInMicroseconds = 0 /*15000*/;
    private const int SleepDelayInMicroseconds = 700;
    private Game CurrentGame { get; set; } = null!;
    private string LastPlacedSymbolText { get; set; } = "";

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

            RunGame();
            (exit, sameConfig) = GameFinishedChoiceDialog();
        } while (!exit);
    }

    private void Setup()
    {
        GameModeOption chosenGameModeOption = SelectGameMode();
        if (SkipShapeSelection())
        {
            CurrentGame = chosenGameModeOption == GameModeOption.PlayerVersusPlayer
                ? new Game(new Player('X'), new Player('O'))
                : new CpuGame(new Player('X'), new Cpu('O'));
            return;
        }

        (Player player1, Player player2) = SelectShapes(chosenGameModeOption);

        CurrentGame = chosenGameModeOption switch
        {
            GameModeOption.PlayerVersusPlayer => new Game(player1, player2),
            GameModeOption.PlayerVersusCpu when player2 is Cpu cpu => new CpuGame(player1, cpu),
            _ => throw new InvalidOperationException("Invalid game mode or player configuration")
        };
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
            PerformCpuMove();
        }
        else
        {
            PerformPlayerMove();
        }
    }

    private ConsoleKey ReadValidConsoleKey(params ConsoleKey[] validKeys)
    {
        if (validKeys.Length == 0)
        {
            throw new ArgumentException("At least one valid key must be provided.");
        }

        ConsoleKey choice;
        do
        {
            choice = ReadInputKey();
        } while (!validKeys.Contains(choice));

        return choice;
    }

    private ConsoleKey ReadValidGameOption(params GameModeOption[] validOptions)
    {
        ConsoleKey[] validKeys = validOptions.Select(GameOptionKeyMapper.GameModeToConsoleKey).ToArray();
        return ReadValidConsoleKey(validKeys);
    }

    private ConsoleKey ReadValidPlayAgainOption(params PlayAgainOption[] validOptions)
    {
        ConsoleKey[] validKeys = validOptions.Select(GameOptionKeyMapper.PlayAgainOptionToConsoleKey).ToArray();
        return ReadValidConsoleKey(validKeys);
    }

    private ConsoleKey ReadValidSelectShapeOption(params SelectShapesOption[] validOptions)
    {
        ConsoleKey[] validKeys = validOptions.Select(GameOptionKeyMapper.SelectShapeOptionToConsoleKey).ToArray();
        return ReadValidConsoleKey(validKeys);
    }

    private (bool exitGame, bool playAgainWithSameConfigs) GameFinishedChoiceDialog()
    {
        SlowPrint("\nWould you like to play again?\n");
        DisplayOptions();
        ConsoleKey userChoice = ReadValidPlayAgainOption(
            PlayAgainOption.PlayAgainWithSameConfig,
            PlayAgainOption.PlayAgainWithNewConfig,
            PlayAgainOption.ExitGame
        );

        return ProcessChoice(userChoice);

        void DisplayOptions()
        {
            Console.WriteLine("Play again with same configurations - (Press 1)");
            Console.WriteLine("Play again with different configurations - (Press 2)");
            Console.WriteLine("Exit game - (Press 3)\n");
        }

        (bool exitGame, bool playAgainWithSameConfigs) ProcessChoice(ConsoleKey choice)
        {
            GameOptionKeyMapper.PlayAgainOptionToConsoleKey(PlayAgainOption.PlayAgainWithNewConfig);
            GameOptionKeyMapper.PlayAgainOptionToConsoleKey(PlayAgainOption.ExitGame);

            if (choice == GameOptionKeyMapper.PlayAgainOptionToConsoleKey(PlayAgainOption.PlayAgainWithSameConfig))
            {
                CurrentGame.ResetGame();
                return (exitGame: false, playAgainWithSameConfigs: true);
            }

            if (choice == GameOptionKeyMapper.PlayAgainOptionToConsoleKey(PlayAgainOption.PlayAgainWithNewConfig))
            {
                return (exitGame: false, playAgainWithSameConfigs: false);
            }

            if (choice == GameOptionKeyMapper.PlayAgainOptionToConsoleKey(PlayAgainOption.ExitGame))
            {
                return (exitGame: true, playAgainWithSameConfigs: false);
            }

            throw new InvalidOperationException("Invalid choice");
        }
    }

    private void RunGame()
    {
        CommencingGameMessage();

        MainGameLoop();

        Thread.Sleep(2500);
    }

    private void DefaultCurrentPlayerTurnMessage()
    {
        Console.WriteLine("\n");
        SlowPrint($"{CurrentGame.CurrentPlayer} has the current turn (Type h to display controls):\n");
    }

    private void DisplayControls()
    {
        const int sleepTime = 2500;
        const int sleepTimeForPositions = 1200;
        const int delayTime = 25000;

        Console.WriteLine("--------------------------------------");
        SlowPrint(
            "There are two ways to specify a move.", delayTime, noSleep: true);
        Console.WriteLine("--------------------------------------");

        Thread.Sleep(sleepTime);
        SlowPrint("The first way is to type a single number - e.g: 1", delayTime);
        Thread.Sleep(sleepTime);
        SlowPrint(
            "To help you understand, the number corresponding to the four corners and middle position will be listed:",
            delayTime);
        Thread.Sleep(sleepTime);
        Console.WriteLine("--------------------------------");
        SlowPrint($"Top left corner would be {(GridPosition)GridPosition.TopLeft}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Top right corner would be {(GridPosition)GridPosition.TopRight}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Middle position would be {(GridPosition)GridPosition.Middle}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom left corner would be {(GridPosition)GridPosition.BottomLeft}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom right corner would be {(GridPosition)GridPosition.BottomRight}", delayTime, noSleep: true);
        Console.WriteLine("--------------------------------");

        Thread.Sleep(sleepTime);

        SlowPrint("The second way is to type a coordinate - e.g: 1,1 / 1.1", delayTime);
        Thread.Sleep(sleepTime);
        SlowPrint(
            "The coordinate corresponding to the four corners and middle position will be listed:", delayTime);
        Thread.Sleep(sleepTime);
        Console.WriteLine("--------------------------------");
        SlowPrint($"Top left corner would be {GridPosition.TopLeft}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Top right corner would be {GridPosition.TopRight}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Middle position would be {GridPosition.Middle}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom left corner would be {GridPosition.BottomLeft}", delayTime,
            sleepDelayInMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom right corner would be {GridPosition.BottomRight}", delayTime, noSleep: true);
        Console.WriteLine("--------------------------------");
        SlowPrint("When a move has been specified, press enter to perform the move.\n");

        SlowPrint("Press enter when you're ready to return to the game...");
        Console.WriteLine();

        ReadValidConsoleKey(ConsoleKey.Enter);
    }

    private string PlayerPlacedSymbolMessage(Player player, IntegerPair pair)
    {
        return PlayerPlacedSymbolMessage($"{player} placed a symbol on {(GridPosition)pair} / {pair}");
    }

    private string PlayerPlacedSymbolMessage(string text)
    {
        SlowPrint(text);
        return text;
    }

    private void DefaultWrongFormatMessage(string text)
    {
        SlowPrint(text);
        Thread.Sleep(500);
        SlowPrint("Try again...");
        Thread.Sleep(1500);
        DisplayMoveResult(lastMoveResultText: LastPlacedSymbolText);
    }

    private void PerformCpuMove()
    {
        if (CurrentGame is not CpuGame cpuGame || CurrentGame.CurrentPlayer is not Cpu cpu)
        {
            throw new InvalidOperationException("PerformCpuMove called with invalid game or player type.");
        }

        IntegerPair movePosition = GetOptimalMove() ?? GetRandomValidMove();

        ApplyMove(movePosition);
        DisplayMoveResult(movePosition);
        return;

        IntegerPair? GetOptimalMove()
        {
            return cpuGame.CpuCanWin(out var winningMove) ? winningMove :
                cpuGame.CpuCanLose(out var blockingMove) ? blockingMove :
                null;
        }

        IntegerPair GetRandomValidMove()
        {
            IntegerPair move;
            do
            {
                move = cpu.GetRandomPosition();
            } while (IsSpaceOccupied(move));

            return move;
        }

        void ApplyMove(IntegerPair position)
        {
            cpu.AddSymbolPosition(position);
            Console.WriteLine();
            CurrentGame.GameGrid[position.First, position.Second] = cpu.Symbol;
        }
    }

    private void PerformPlayerMove()
    {
        while (true)
        {
            DefaultCurrentPlayerTurnMessage();

            IntegerPair? move = GetValidPlayerMove();

            // Todo - Do something else than use null
            if (move == null)
            {
                continue;
            }

            ApplyMove(move.Value);
            return;
        }

        void ApplyMove(IntegerPair move)
        {
            CurrentGame.CurrentPlayer.AddSymbolPosition(move);
            CurrentGame.GameGrid[move] = CurrentGame.CurrentPlayer.Symbol;
            DisplayMoveResult(move);
        }
    }

    private IntegerPair? GetValidPlayerMove()
    {
        string? input = ReadInput();

        if (input == null)
        {
            return InvalidMove("An unexpected action was performed.");
        }

        if (input.Equals("h"))
        {
            DisplayControls();
            DisplayMoveResult(lastMoveResultText: LastPlacedSymbolText);
            return null;
        }

        IntegerPair? move = ParseMove(input);
        if (move == null)
        {
            return InvalidMove("The move was written in an incorrect format.");
        }

        if (!IsWithinGrid(move.Value))
        {
            return InvalidMove("You are trying to place a symbol outside of the grid.");
        }

        if (IsSpaceOccupied(move.Value))
        {
            return InvalidMove("The space is already being occupied.");
        }

        return move;

        IntegerPair? InvalidMove(string message)
        {
            DefaultWrongFormatMessage(message);
            return null;
        }
    }

    private IntegerPair? ParseMove(string input)
    {
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
            }
            else
            {
                if (!TryParse(input, out int parsedNumber))
                {
                    return null;
                }

                GridPosition number = new GridPosition(parsedNumber - 1);
                return (IntegerPair)number;
            }
        }
        catch
        {
            return null;
        }

        return pair;
    }

    private void DisplayMoveResult(IntegerPair? move, string? lastMoveResultText = null)
    {
        Console.Write(CurrentGame.GameGrid);

        if (move.HasValue)
        {
            LastPlacedSymbolText = PlayerPlacedSymbolMessage(CurrentGame.CurrentPlayer, move.Value);
        }
        else if (lastMoveResultText != null)
        {
            PlayerPlacedSymbolMessage(lastMoveResultText);
        }

        Thread.Sleep(1000);
    }

    private void DisplayMoveResult(string lastMoveResultText)
    {
        DisplayMoveResult(move: null, lastMoveResultText: lastMoveResultText);
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

    private GameModeOption SelectGameMode()
    {
        SlowPrint("How would you like to play?");

        Console.WriteLine("Alone versus a CPU - (Press 1)");
        Console.WriteLine("Versus another local player - (Press 2)\n");

        ConsoleKey chosenKey =
            ReadValidGameOption(
                GameModeOption.PlayerVersusCpu,
                GameModeOption.PlayerVersusPlayer
            );

        GameModeOption chosenGameModeOption =
            chosenKey == GameOptionKeyMapper.GameModeToConsoleKey(GameModeOption.PlayerVersusCpu)
                ? GameModeOption.PlayerVersusCpu
                : GameModeOption.PlayerVersusPlayer;

        string modeDescription = chosenGameModeOption == GameModeOption.PlayerVersusCpu ? "a CPU" : "a local player";

        SlowPrint($"\nYou have chosen to play versus {modeDescription}.\n\n");

        return chosenGameModeOption;
    }

    private (Player player1, Player player2) SelectShapes(GameModeOption gameModeOption)
    {
        string player1Prompt = gameModeOption == GameModeOption.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 1?"
            : "Which shape would you like for yourself to be?";

        char shape1 = GetShapeInput(player1Prompt);

        SlowPrint(gameModeOption == GameModeOption.PlayerVersusPlayer
            ? $"Player 1 chose the shape {shape1}."
            : $"You chose the shape {shape1} for yourself.");

        Console.WriteLine();

        string player2Prompt = gameModeOption == GameModeOption.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 2?"
            : "Which shape would you like for the CPU to be?";

        char shape2 = GetShapeInput(player2Prompt, shape1);

        SlowPrint(gameModeOption == GameModeOption.PlayerVersusPlayer
            ? $"Player 2 chose the shape {shape2}"
            : $"You chose the shape {shape2} for the CPU");

        Console.WriteLine();

        return (new Player(shape1),
            gameModeOption == GameModeOption.PlayerVersusPlayer ? new Player(shape2) : new Cpu(shape2));

        char GetShapeInput(string prompt, char? excludeShape = null)
        {
            SlowPrint(prompt);
            SlowPrint("Press the button with the corresponding letter, from the alphabet, to choose your shape.");

            char shape;
            do
            {
                ConsoleKeyInfo keyPressed = ReadInputKeyInfo();
                shape = char.ToUpper(keyPressed.KeyChar);
            } while (!char.IsLetter(shape) || shape == excludeShape);

            Console.WriteLine();
            return shape;
        }
    }

    private bool IsSpaceOccupied(IntegerPair pair)
    {
        return !CurrentGame.GameGrid.IsSpaceAvailable(pair.First, pair.Second);
    }

    private bool IsWithinGrid(IntegerPair pair)
    {
        return !(pair.First is >= Game.GameGridSideLength or < 0 ||
                 pair.Second is >= Game.GameGridSideLength or < 0);
    }

    private bool SkipShapeSelection()
    {
        SlowPrint("Do you wish to select your shapes?");

        Console.WriteLine("No - (Press 1)");
        Console.WriteLine("Yes - (Press 2)\n");
        ConsoleKey selectShapeButton =
            ReadValidSelectShapeOption(SelectShapesOption.SelectCustomShapes, SelectShapesOption.UseDefaultShapes);

        return selectShapeButton ==
               GameOptionKeyMapper.SelectShapeOptionToConsoleKey(SelectShapesOption.UseDefaultShapes);
    }

    private void SlowPrint(string text, int textDelayInMicroseconds = TextDelayInMicroseconds,
        int sleepDelayInMicroseconds = SleepDelayInMicroseconds, bool noSleep = false)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(TimeSpan.FromMicroseconds(textDelayInMicroseconds));
        }

        Console.WriteLine();

        if (!noSleep)
        {
            Thread.Sleep(sleepDelayInMicroseconds);
        }
    }

    private void CommencingGameMessage()
    {
        SlowPrint("The game will now commence...");
        Thread.Sleep(1000);
        Console.WriteLine();
        Console.WriteLine(CurrentGame.GameGrid);
        SlowPrint($"{CurrentGame.CurrentPlayer} will start the turn...\n");
        LastPlacedSymbolText = $"{CurrentGame.CurrentPlayer} will start the turn...";
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