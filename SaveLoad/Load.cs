using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Crossword.SaveLoad;

public class Load
{
    public static void Get(List<Cell> listAllCellStruct, string[] listEmptyCellStruct)
    {
        foreach (Cell cell in listAllCellStruct)
        {
            cell.label.Content = null;
            cell.border.Background = Brushes.Black;
        }

        foreach (var item in listEmptyCellStruct)
        {
            List<string> strings = new List<string>(item.Split(';'));
            int x = Int32.Parse(strings[0]);
            int y = Int32.Parse(strings[1]);
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.x == x)
                {
                    if (cell.y == y)
                    {
                        cell.border.Background = Brushes.Transparent;
                    }
                }
            }
        }
    }
}