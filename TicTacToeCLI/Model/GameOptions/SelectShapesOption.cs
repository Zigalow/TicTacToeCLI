using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

/// <summary>
/// Represents options for selecting shapes in the Tic-Tac-Toe game.
/// </summary>
public enum SelectShapesOption
{
    /// <summary>
    /// Represents options for selecting shapes in the Tic-Tac-Toe game.
    /// </summary>
    [Description("Don't select shapes and play with default shapes")]
    UseDefaultShapes,

    /// <summary>
    /// Allow players to select custom shapes.
    /// </summary>
    [Description("Select which shapes to play with")]
    SelectCustomShapes
}