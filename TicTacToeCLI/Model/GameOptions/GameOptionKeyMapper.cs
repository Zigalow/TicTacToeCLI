namespace TicTacToeCLI.Model.GameOptions;

public static class GameOptionKeyMapper
{
    public static ConsoleKey GameModeToConsoleKey(GameModeOption option)
    {
        return option switch
        {
            GameModeOption.PlayerVersusCpu => ConsoleKey.D1,
            GameModeOption.PlayerVersusPlayer => ConsoleKey.D2,
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }

    public static ConsoleKey PlayAgainOptionToConsoleKey(PlayAgainOption option)
    {
        return option switch
        {
            PlayAgainOption.PlayAgainWithSameConfig => ConsoleKey.D1,
            PlayAgainOption.PlayAgainWithNewConfig => ConsoleKey.D2,
            PlayAgainOption.ExitGame => ConsoleKey.D3,
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }

    public static ConsoleKey SelectShapeOptionToConsoleKey(SelectShapesOption option)
    {
        return option switch
        {
            SelectShapesOption.DenySelectShape => ConsoleKey.D1,
            SelectShapesOption.AcceptSelectShape => ConsoleKey.D2,
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }
}