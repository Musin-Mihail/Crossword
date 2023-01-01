using System.Collections.Generic;
using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static void Get(List<Cell> listAllCellStruct)
    {
        Global.listEmptyCellStruct.Clear();
        foreach (Cell cell in listAllCellStruct)
        {
            if (cell.border.Background == Brushes.Transparent)
            {
                Global.listEmptyCellStruct.Add(cell);
            }
        }
    }
}