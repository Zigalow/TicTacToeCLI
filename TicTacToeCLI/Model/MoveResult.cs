namespace TicTacToeCLI.Model;

/// <summary>
/// Represents the result of parsing and validating a player's move input.
/// </summary>
public class MoveResult
{
    /// <summary>
    /// Gets the status of the attempted move.
    /// </summary>
    public MoveStatus Status { get; }

    /// <summary>
    /// Gets the parsed move as a <see cref="GridCoordinate"/>, if available.
    /// </summary>
    /// <remarks>
    /// This will be null for move statuses that don't represent a valid move,
    /// such as UnexpectedInput, DisplayHelp, or InvalidFormat.
    /// </remarks>
    public GridCoordinate? Move { get; }

    /// <summary>
    /// Initializes a new instance of the MoveResult class.
    /// </summary>
    /// <param name="status">The status of the attempted move.</param>
    /// <param name="move">The parsed move as a GridCoordinate, if available.</param>
    public MoveResult(MoveStatus status, GridCoordinate? move = null)
    {
        Status = status;
        Move = move;
    }
}

/// <summary>
/// Represents the various statuses that can result from a player's move attempt.
/// </summary>
public enum MoveStatus
{
    /// <summary>
    /// Indicates that the input was unexpected or could not be interpreted.
    /// </summary>
    UnexpectedInput,

    /// <summary>
    /// Indicates that the player has requested to display control information.
    /// </summary>
    DisplayControls,

    /// <summary>
    /// Indicates that the input was in an incorrect format and could not be parsed as a move.
    /// </summary>
    InvalidFormat,

    /// <summary>
    /// Indicates that the parsed move is outside the boundaries of the game grid.
    /// </summary>
    OutOfBounds,

    /// <summary>
    /// Indicates that the parsed move refers to a space that is already occupied.
    /// </summary>
    SpaceOccupied,

    /// <summary>
    /// Indicates that the parsed move is valid and can be played.
    /// </summary>
    Valid
}