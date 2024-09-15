namespace TicTacToeCLI.Model;

/// <summary>
/// The EvenGridSizeException class inherits from the Exception class and represents an exception that is thrown when the grid size is not an odd number.
/// </summary>
public class EvenGridSizeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the EvenGridSizeException class with a default message stating "Grid size must be an odd number."
    /// </summary>
    public EvenGridSizeException() : base("Grid size must be an odd number.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the EvenGridSizeException class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public EvenGridSizeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the EvenGridSizeException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public EvenGridSizeException(string message, Exception inner) : base(message, inner)
    {
    }
}