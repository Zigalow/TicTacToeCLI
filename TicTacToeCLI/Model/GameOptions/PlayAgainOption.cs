using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

public enum PlayAgainOption
{
    [Description("Play again with the same configurations")]
    PlayAgainWithSameConfig,

    [Description("Play again with new configurations")]
    PlayAgainWithNewConfig,

    [Description("Exit the game")]
    ExitGame
}