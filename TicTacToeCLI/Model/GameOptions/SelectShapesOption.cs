using System.ComponentModel;

namespace TicTacToeCLI.Model.GameOptions;

public enum SelectShapesOption
{
    [Description("Don't select shapes and play with default shapes")]
    DenySelectShape,

    [Description("Select which shapes to play with")]
    AcceptSelectShape
}