namespace TicTacToeCLI.Model.GameOptions;

/// <summary>
/// Provides mapping between game options and corresponding console keys.
/// </summary>
public static class GameOptionKeyMapper
{
    /// <summary>
    /// Maps a GameModeOption to its corresponding ConsoleKey.
    /// </summary>
    /// <param name="option">The GameModeOption to map.</param>
    /// <returns>The corresponding ConsoleKey.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid GameModeOption is provided.</exception>
    public static ConsoleKey GameModeToConsoleKey(GameModeOption option)
    {
        return option switch
        {
            GameModeOption.PlayerVersusCpu => ConsoleKey.D1,
            GameModeOption.PlayerVersusPlayer => ConsoleKey.D2,
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }

    /// <summary>
    /// Maps a PlayAgainOption to its corresponding ConsoleKey.
    /// </summary>
    /// <param name="option">The PlayAgainOption to map.</param>
    /// <returns>The corresponding ConsoleKey.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid PlayAgainOption is provided.</exception>
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

    /// <summary>
    /// Maps a SelectShapesOption to its corresponding ConsoleKey.
    /// </summary>
    /// <param name="option">The SelectShapesOption to map.</param>
    /// <returns>The corresponding ConsoleKey.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid SelectShapesOption is provided.</exception>
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