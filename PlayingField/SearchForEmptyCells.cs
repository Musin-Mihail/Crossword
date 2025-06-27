using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static void Get()
    {
        App.GameState.ListEmptyCellStruct.Clear();
        foreach (var cell in App.GameState.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Transparent)
            {
                App.GameState.ListEmptyCellStruct.Add(cell);
            }
        }
    }
}