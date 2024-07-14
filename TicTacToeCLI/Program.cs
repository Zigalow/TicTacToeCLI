using TicTacToeCLI.Controller;

namespace TicTacToeCLI;

internal abstract class Program
{
    private static void Main()
    {
        GameController gameController = new();
        gameController.Start();
        Console.WriteLine("Press a key to exit...");
        Console.ReadKey();
    }
}