namespace Crossword.Models;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public string? Content { get; set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
}