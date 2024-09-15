using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

/// <summary>
/// Represents options for continuing or ending the game after a match.
/// </summary>
public enum PlayAgainOption
{
    /// <summary>
    /// Play another game with the same configuration.
    /// </summary>
    [Description("Play again with the same configurations")]
    PlayAgainWithSameConfig,

    /// <summary>
    /// Play another game with a new configuration.
    /// </summary>
    [Description("Play again with new configurations")]
    PlayAgainWithNewConfig,

    /// <summary>
    /// End the game session.
    /// </summary>
    [Description("Exit the game")]
    ExitGame
}