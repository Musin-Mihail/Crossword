using System;
using System.Collections.Generic;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.SaveLoad;

public class Load
{
    public static void Get(string[] listEmptyCellStruct)
    {
        foreach (Cell cell in Global.listAllCellStruct)
        {
            cell.label.Content = null;
            cell.border.Background = Brushes.Black;
        }

        foreach (var item in listEmptyCellStruct)
        {
            List<string> strings = new List<string>(item.Split(';'));
            int x = Int32.Parse(strings[0]);
            int y = Int32.Parse(strings[1]);
            foreach (Cell cell in Global.listAllCellStruct)
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