using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

/// <summary>
/// Mode of the game being played
/// </summary>
public enum GameModeOption
{
    /// <summary>
    /// Game mode of player playing against the CPU 
    /// </summary>
    [Description("Player versus a CPU")]
    PlayerVersusCpu,

    /// <summary>
    /// Game mode of player playing against another local player
    /// </summary>
    [Description("Player versus another local player")]
    PlayerVersusPlayer,
}