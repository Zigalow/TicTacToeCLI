using TicTacToeCLI.Controller;

namespace TicTacToeCLI;

abstract class Program
{
    static void Main()
    {
        GameController gameController = new();
        gameController.Start();
        Console.WriteLine("Press a key to exit...");
        Console.ReadKey();
    }
}