namespace TicTacToeCLI.Model;

public class IntegerPair
{
    private readonly int _first;
    private readonly int _second;
    
    public int First
    {
        get => _first;
        private init => _first = value;
    }
    public int Second
    {
        get => _second;
        private init => _second = value;
    }
    
    public IntegerPair(int first, int second)
    {
        First = first;
        Second = second;
    }

    public override string ToString()
    {
        return $"{_first+1}.{_second+1}";
    }
}