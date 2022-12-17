using System.Collections.Generic;
using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static List<Cell> Get(List<Cell> listAllCellStruct)
    {
        List<Cell> listEmptyCellStruct = new List<Cell>();
        foreach (Cell cell in listAllCellStruct)
        {
            if (cell.border.Background == Brushes.Transparent)
            {
                listEmptyCellStruct.Add(cell);
            }
        }

        return listEmptyCellStruct;
    }
}