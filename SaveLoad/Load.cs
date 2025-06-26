using System.Collections.Generic;
using System.Windows.Media;

namespace Crossword.SaveLoad;

public class Load
{
    public static void Get(string[] listEmptyCellStruct)
    {
        foreach (var cell in Global.ListAllCellStruct)
        {
            cell.Label.Content = null;
            cell.Border.Background = Brushes.Black;
        }

        foreach (var item in listEmptyCellStruct)
        {
            var strings = new List<string>(item.Split(';'));
            var x = int.Parse(strings[0]);
            var y = int.Parse(strings[1]);
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.X == x)
                {
                    if (cell.Y == y)
                    {
                        cell.Border.Background = Brushes.Transparent;
                    }
                }
            }
        }
    }
}