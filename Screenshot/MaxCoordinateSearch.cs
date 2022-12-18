using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Crossword.Screenshot;

public class MaxCoordinateSearch
{
    public static void Get(List<Cell> listCell, ref int topMaxX, ref int downMaxX, ref int leftMaxY, ref int rightMaxY)
    {
        topMaxX = 99;
        leftMaxY = 99;
        downMaxX = 0;
        rightMaxY = 0;
        foreach (Cell cell in listCell)
        {
            if (cell.border.Background == Brushes.Transparent)
            {
                if (cell.x < topMaxX)
                {
                    topMaxX = cell.x;
                }

                if (cell.y < leftMaxY)
                {
                    leftMaxY = cell.y;
                }

                if (cell.x > downMaxX)
                {
                    downMaxX = cell.x;
                }

                if (cell.y > rightMaxY)
                {
                    rightMaxY = cell.y;
                }
            }
        }
    }  
}