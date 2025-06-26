using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static void Get()
    {
        Global.ListEmptyCellStruct.Clear();
        foreach (var cell in Global.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Transparent)
            {
                Global.ListEmptyCellStruct.Add(cell);
            }
        }
    }
}