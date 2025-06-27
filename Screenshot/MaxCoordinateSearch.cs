using System.Windows.Media;

namespace Crossword.Screenshot;

public class MaxCoordinateSearch
{
    public static void Get(ref int topMaxX, ref int downMaxX, ref int leftMaxY, ref int rightMaxY, CrosswordState gameState)
    {
        topMaxX = 99;
        leftMaxY = 99;
        downMaxX = 0;
        rightMaxY = 0;
        foreach (var cell in gameState.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Transparent)
            {
                if (cell.X < topMaxX) topMaxX = cell.X;
                if (cell.Y < leftMaxY) leftMaxY = cell.Y;
                if (cell.X > downMaxX) downMaxX = cell.X;
                if (cell.Y > rightMaxY) rightMaxY = cell.Y;
            }
        }
    }
}