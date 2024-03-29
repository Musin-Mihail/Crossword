﻿using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.Screenshot;

public class MaxCoordinateSearch
{
    public static void Get(ref int topMaxX, ref int downMaxX, ref int leftMaxY, ref int rightMaxY)
    {
        topMaxX = 99;
        leftMaxY = 99;
        downMaxX = 0;
        rightMaxY = 0;
        foreach (Cell cell in Global.listAllCellStruct)
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