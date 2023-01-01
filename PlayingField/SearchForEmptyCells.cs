using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static void Get()
    {
        Global.listEmptyCellStruct.Clear();
        foreach (Cell cell in Global.listAllCellStruct)
        {
            if (cell.border.Background == Brushes.Transparent)
            {
                Global.listEmptyCellStruct.Add(cell);
            }
        }
    }
}