using TicTacToeCLI.Model;
using TicTacToeCLI.Model.GameOptions;
using static System.Int32;

// Todo - XML-doc and code refactoring (code analysis) - [in progress]
// Todo - Add readme

// Todo - Auto fills the spots when it's a tie (maybe)
// Todo - Setup configurations in github, so people can't accept their own pull request
namespace TicTacToeCLI.Controller;

/// <summary>
/// Controls the flow and logic of the Tic-Tac-Toe game.
/// </summary>
public class GameController
{
    private const int CharPrintDelayMicroseconds = 15000;
    private const int PostPrintDelayMicroseconds = 700;
    private Game CurrentGame { get; set; } = null!;
    private string LastPlacedSymbolText { get; set; } = "";

    /// <summary>
    /// Initiates a new game session, including game setup and execution.
    /// </summary>
    /// <remarks>
    /// This method controls the main game loop, allowing players to start new games
    /// or continue with the same configuration until they choose to exit.
    /// </remarks>
    public void InitiateGameSession()
    {
        DisplayWelcomeMessage();
        bool useExistingConfiguration = false;
        bool shouldExitGame;

        do
        {
            if (!useExistingConfiguration)
            {
                ConfigureGameSettings();
            }

            PlayGame();
            (shouldExitGame, useExistingConfiguration) = EndGameChoiceDialog();
        } while (!shouldExitGame);
    }

    private void DisplayWelcomeMessage()
    {
        if (Console.KeyAvailable)
        {
        } // Prevents the user from typing while welcome message is being printed

        SlowPrint("Welcome to TicTacToe", 3000);
        Thread.Sleep(2000);
        SlowPrint("I hope you enjoy the experience\n\n", 3000);
        Thread.Sleep(1000);
    }

    /// <summary>
    /// Configures the game settings based on user input.
    /// </summary>
    /// <remarks>
    /// This method handles the selection of game mode (Player vs Player or Player vs CPU)
    /// and allows players to choose custom shapes or use default ones.
    /// </remarks>
    private void ConfigureGameSettings()
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

    private GameModeOption SelectGameMode()
    {
        SlowPrint("How would you like to play?");

        Console.WriteLine("Alone versus a CPU - (Press 1)");
        Console.WriteLine("Versus another local player - (Press 2)\n");

        ConsoleKey selectedGameModeKey =
            ReadValidGameOption(
                GameModeOption.PlayerVersusCpu,
                GameModeOption.PlayerVersusPlayer
            );

        GameModeOption chosenGameModeOption =
            selectedGameModeKey == GameOptionKeyMapper.GameModeToConsoleKey(GameModeOption.PlayerVersusCpu)
                ? GameModeOption.PlayerVersusCpu
                : GameModeOption.PlayerVersusPlayer;

        string modeDescription = chosenGameModeOption == GameModeOption.PlayerVersusCpu ? "a CPU" : "a local player";

        SlowPrint($"\nYou have chosen to play versus {modeDescription}.\n\n");

        return chosenGameModeOption;
    }

    private bool SkipShapeSelection()
    {
        SlowPrint("Do you wish to select your shapes?");

        Console.WriteLine("No - (Press 1)");
        Console.WriteLine("Yes - (Press 2)\n");
        ConsoleKey shapeSelectionKey =
            ReadValidSelectShapeOption(SelectShapesOption.SelectCustomShapes, SelectShapesOption.UseDefaultShapes);

        return shapeSelectionKey ==
               GameOptionKeyMapper.SelectShapeOptionToConsoleKey(SelectShapesOption.UseDefaultShapes);
    }

    private (Player player1, Player player2) SelectShapes(GameModeOption gameModeOption)
    {
        string player1Prompt = gameModeOption == GameModeOption.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 1?"
            : "Which shape would you like for yourself to be?";

        char player1Symbol = GetShapeInput(player1Prompt);

        SlowPrint(gameModeOption == GameModeOption.PlayerVersusPlayer
            ? $"Player 1 chose the shape {player1Symbol}."
            : $"You chose the shape {player1Symbol} for yourself.");

        Console.WriteLine();

        string player2Prompt = gameModeOption == GameModeOption.PlayerVersusPlayer
            ? "Which shape would you like to be, Player 2?"
            : "Which shape would you like for the CPU to be?";

        char player2Symbol = GetShapeInput(player2Prompt, player1Symbol);

        SlowPrint(gameModeOption == GameModeOption.PlayerVersusPlayer
            ? $"Player 2 chose the shape {player2Symbol}"
            : $"You chose the shape {player2Symbol} for the CPU");

        Console.WriteLine();

        return (new Player(player1Symbol),
            gameModeOption == GameModeOption.PlayerVersusPlayer ? new Player(player2Symbol) : new Cpu(player2Symbol));

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

    private void PlayGame()
    {
        CommencingGameMessage();

        ExecuteGameRounds();

        Thread.Sleep(2500);
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

    /// <summary>
    /// Executes the main game loop, handling turns and checking for win/draw conditions.
    /// </summary>
    /// <remarks>
    /// This method continues to execute turns until a player wins or the game ends in a draw.
    /// It also handles the transition between players and updates the game state.
    /// </remarks>
    private void ExecuteGameRounds()
    {
        while (true)
        {
            ExecuteCurrentTurn();

            if (CurrentGame.HasCurrentPlayerWon())
            {
                SlowPrint($"\nCongratulations to {CurrentGame.CurrentPlayer} on winning the game...");
                return;
            }

            if (CurrentGame.IsGameDrawn())
            {
                SlowPrint("\nThere are no available spaces left, and the game has ended in a tie...");
                return;
            }

            AdvanceToNextPlayer();
        }
    }

    /// <summary>
    /// Executes a single turn for the current player.
    /// </summary>
    private void ExecuteCurrentTurn()
    {
        if (CurrentGame.CurrentPlayer is Cpu)
        {
            ExecuteCpuMove();
        }
        else
        {
            ExecutePlayerMove();
        }
    }

    private void ExecuteCpuMove()
    {
        if (CurrentGame is not CpuGame cpuGame || CurrentGame.CurrentPlayer is not Cpu cpu)
        {
            throw new InvalidOperationException("PerformCpuMove called with invalid game or player type.");
        }

        GridCoordinate movePosition = GetOptimalMove() ?? GetRandomValidMove();

        ApplyCpuMove(movePosition);
        DisplayMoveResult(movePosition);
        return;

        GridCoordinate? GetOptimalMove()
        {
            return cpuGame.CpuCanWin(out var winningMove) ? winningMove :
                cpuGame.CpuCanLose(out var blockingMove) ? blockingMove :
                null;
        }

        GridCoordinate GetRandomValidMove()
        {
            GridCoordinate move;
            do
            {
                move = cpu.GetRandomPosition();
            } while (IsSpaceOccupied(move));

            return move;
        }

        void ApplyCpuMove(GridCoordinate position)
        {
            cpu.AddSymbolPosition(position);
            Console.WriteLine();
            CurrentGame.GameGrid[position.First, position.Second] = cpu.Symbol;
        }
    }

    private void ExecutePlayerMove()
    {
        while (true)
        {
            CurrentPlayerTurnMessage();

            MoveResult moveResult = ProcessPlayerMoveInput();

            switch (moveResult.Status)
            {
                case MoveStatus.DisplayControls:
                    DisplayControls();
                    DisplayLastMoveResult();
                    continue;
                case MoveStatus.Valid:
                    ApplyPlayerMove(moveResult.Move!.Value);
                    return;
                default:
                    GenerateErrorMessage(moveResult.Status);
                    continue;
            }
        }

        void ApplyPlayerMove(GridCoordinate move)
        {
            CurrentGame.CurrentPlayer.AddSymbolPosition(move);
            CurrentGame.GameGrid[move] = CurrentGame.CurrentPlayer.Symbol;
            DisplayMoveResult(move);
        }
    }

    private void GenerateErrorMessage(MoveStatus moveStatus)
    {
        string errorMessage = moveStatus switch
        {
            MoveStatus.OutOfBounds => "You are trying to place a symbol outside of the grid.",
            MoveStatus.SpaceOccupied => "The space is already being occupied.",
            MoveStatus.InvalidFormat => "The move was written in an incorrect format.",
            _ => "An unexpected error occurred."
        };

        DisplayErrorAndPromptRetry(errorMessage);
    }

    private void DisplayErrorAndPromptRetry(string text)
    {
        SlowPrint(text);
        Thread.Sleep(500);
        SlowPrint("Try again...");
        Thread.Sleep(1500);
        DisplayLastMoveResult();
    }

    private void DisplayControls()
    {
        const int sleepTime = 2500;
        const int sleepTimeForPositions = 1200;
        const int delayTime = 25000;

        Console.WriteLine("--------------------------------------");
        SlowPrint(
            "There are two ways to specify a move.", delayTime, skipPostPrintDelay: true);
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
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Top right corner would be {(GridPosition)GridPosition.TopRight}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Middle position would be {(GridPosition)GridPosition.Middle}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom left corner would be {(GridPosition)GridPosition.BottomLeft}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom right corner would be {(GridPosition)GridPosition.BottomRight}", delayTime,
            skipPostPrintDelay: true);
        Console.WriteLine("--------------------------------");

        Thread.Sleep(sleepTime);

        SlowPrint("The second way is to type a coordinate - e.g: 1,1 / 1.1", delayTime);
        Thread.Sleep(sleepTime);
        SlowPrint(
            "The coordinate corresponding to the four corners and middle position will be listed:", delayTime);
        Thread.Sleep(sleepTime);
        Console.WriteLine("--------------------------------");
        SlowPrint($"Top left corner would be {GridPosition.TopLeft}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Top right corner would be {GridPosition.TopRight}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Middle position would be {GridPosition.Middle}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom left corner would be {GridPosition.BottomLeft}", delayTime,
            postPrintDelayMicroseconds: sleepTimeForPositions);
        SlowPrint($"Bottom right corner would be {GridPosition.BottomRight}", delayTime, skipPostPrintDelay: true);
        Console.WriteLine("--------------------------------");
        SlowPrint("When a move has been specified, press enter to perform the move.\n");

        SlowPrint("Press enter when you're ready to return to the game...");
        Console.WriteLine();

        ReadValidConsoleKey(ConsoleKey.Enter);
    }

    private void CurrentPlayerTurnMessage()
    {
        Console.WriteLine("\n");
        SlowPrint($"{CurrentGame.CurrentPlayer} has the current turn (Type h to display controls):\n");
    }

    /// <summary>
    /// Processes and validates the player's move input.
    /// </summary>
    /// <returns>A MoveResult containing the status and parsed move.</returns>
    /// <remarks>
    /// This method handles various input scenarios, including displaying controls,
    /// parsing valid moves, and identifying invalid inputs.
    /// </remarks>
    private MoveResult ProcessPlayerMoveInput()
    {
        string? input = ReadInput();

        if (input == null)
        {
            return new MoveResult(MoveStatus.UnexpectedInput);
        }

        if (input.Equals("h"))
        {
            return new MoveResult(MoveStatus.DisplayControls);
        }

        GridCoordinate? parsedMove = ParseMoveInput(input);
        return ValidateMovePosition(parsedMove);
    }

    /// <summary>
    /// Parses the user's input into a grid coordinate.
    /// </summary>
    /// <param name="input">The user's input string.</param>
    /// <returns>A GridCoordinate if parsing is successful; otherwise, null.</returns>
    private GridCoordinate? ParseMoveInput(string input)
    {
        string[] splitByComma = input.Split(",");
        string[] splitByDot = input.Split(".");

        string[]? parts = splitByComma.Length == 2 ? splitByComma :
            splitByDot.Length == 2 ? splitByDot : null;

        if (parts != null) // If it isn't null, the Integer pair will have length of 2
        {
            if (TryParse(parts[0], out int in1) && TryParse(parts[1], out int in2))
            {
                return new GridCoordinate(in1 - 1, in2 - 1);
            }

            return null;
        }

        if (!TryParse(input, out int parsedNumber))
        {
            return null;
        }

        GridPosition number = new GridPosition(parsedNumber - 1);
        return (GridCoordinate)number;
    }

    /// <summary>
    /// Validates whether a move is written in the correct format, within the grid and on an unoccupied space.
    /// </summary>
    /// <param name="move">The move to validate.</param>
    /// <returns>A MoveResult indicating the validity of the move.</returns>
    private MoveResult ValidateMovePosition(GridCoordinate? move)
    {
        if (move == null)
        {
            return new(MoveStatus.InvalidFormat);
        }

        if (!IsWithinGrid(move.Value))
        {
            return new(MoveStatus.OutOfBounds);
        }

        if (IsSpaceOccupied(move.Value))
        {
            return new(MoveStatus.OutOfBounds);
        }

        return new MoveResult(MoveStatus.Valid, move);
    }

    private void DisplayMoveResult(GridCoordinate move)
    {
        Console.Write(CurrentGame.GameGrid);
        LastPlacedSymbolText = PlayerPlacedSymbolMessage(CurrentGame.CurrentPlayer, move);
        Thread.Sleep(1000);
    }

    private void DisplayLastMoveResult()
    {
        Console.Write(CurrentGame.GameGrid);
        PlayerPlacedSymbolMessage(LastPlacedSymbolText);
        Thread.Sleep(1000);
    }

    private string PlayerPlacedSymbolMessage(Player player, GridCoordinate pair)
    {
        return PlayerPlacedSymbolMessage($"{player} placed a symbol on {(GridPosition)pair} / {pair}");
    }

    private string PlayerPlacedSymbolMessage(string text)
    {
        SlowPrint(text);
        return text;
    }

    private bool IsSpaceOccupied(GridCoordinate pair)
    {
        return !CurrentGame.GameGrid.IsSpaceAvailable(pair.First, pair.Second);
    }

    private bool IsWithinGrid(GridCoordinate pair)
    {
        return !(pair.First is >= Game.GameGridSideLength or < 0 ||
                 pair.Second is >= Game.GameGridSideLength or < 0);
    }

    private void AdvanceToNextPlayer()
    {
        CurrentGame.NextPlayer();
        CurrentGame.IncreaseTurnCounter();
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

    private ConsoleKey ReadValidSelectShapeOption(params SelectShapesOption[] validOptions)
    {
        ConsoleKey[] validKeys = validOptions.Select(GameOptionKeyMapper.SelectShapeOptionToConsoleKey).ToArray();
        return ReadValidConsoleKey(validKeys);
    }

    private ConsoleKey ReadValidPlayAgainOption(params PlayAgainOption[] validOptions)
    {
        ConsoleKey[] validKeys = validOptions.Select(GameOptionKeyMapper.PlayAgainOptionToConsoleKey).ToArray();
        return ReadValidConsoleKey(validKeys);
    }

    private (bool exitGame, bool playAgainWithSameConfigs) EndGameChoiceDialog()
    {
        SlowPrint("\nWould you like to play again?\n");
        DisplayEndGameOptions();
        ConsoleKey userChoice = ReadValidPlayAgainOption(
            PlayAgainOption.PlayAgainWithSameConfig,
            PlayAgainOption.PlayAgainWithNewConfig,
            PlayAgainOption.ExitGame
        );

        return HandleEndGameChoice(userChoice);

        void DisplayEndGameOptions()
        {
            Console.WriteLine("Play again with same configurations - (Press 1)");
            Console.WriteLine("Play again with different configurations - (Press 2)");
            Console.WriteLine("Exit game - (Press 3)\n");
        }

        (bool exitGame, bool playAgainWithSameConfigs) HandleEndGameChoice(ConsoleKey choice)
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

    /// <summary>
    /// Displays text gradually with a delay between each character.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <param name="charPrintDelayMicroseconds">Delay between each character in microseconds.</param>
    /// <param name="postPrintDelayMicroseconds">Delay after printing the entire text in microseconds.</param>
    /// <param name="skipPostPrintDelay">If true, skips the post-print delay.</param>
    /// <remarks>
    /// This method creates a typewriter-like effect for text output, enhancing the user experience.
    /// It allows customization of delays between characters and after the entire text is printed.
    /// </remarks>
    private void SlowPrint(string text, int charPrintDelayMicroseconds = CharPrintDelayMicroseconds,
        int postPrintDelayMicroseconds = PostPrintDelayMicroseconds, bool skipPostPrintDelay = false)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(TimeSpan.FromMicroseconds(charPrintDelayMicroseconds));
        }

        Console.WriteLine();

        if (!skipPostPrintDelay)
        {
            Thread.Sleep(postPrintDelayMicroseconds);
        }
    }

    private string? ReadInput()
    {
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        return Console.ReadLine();
    }

    private ConsoleKey ReadInputKey()
    {
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        return Console.ReadKey(true).Key;
    }

    private ConsoleKeyInfo ReadInputKeyInfo()
    {
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        return Console.ReadKey(true);
    }
}