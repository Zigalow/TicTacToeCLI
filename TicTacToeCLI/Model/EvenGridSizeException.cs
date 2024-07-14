namespace TicTacToeCLI.Model;

public class EvenGridSizeException : Exception
{
    public EvenGridSizeException() : base("Grid size must be an odd number.")
    {
    }

    public EvenGridSizeException(string message) : base(message)
    {
    }

    public EvenGridSizeException(string message, Exception inner) : base(message, inner)
    {
    }
}