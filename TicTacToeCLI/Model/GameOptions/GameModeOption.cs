using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

/// <summary>
/// Represents the available game modes for Tic-Tac-Toe.
/// </summary>
public enum GameModeOption
{
    /// <summary>
    /// Player versus CPU game mode.
    /// </summary>
    [Description("Player versus a CPU")]
    PlayerVersusCpu,

    /// <summary>
    /// Player versus another local player game mode.
    /// </summary>
    [Description("Player versus another local player")]
    PlayerVersusPlayer,
}