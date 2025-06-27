using System.Windows.Media;

namespace Crossword.PlayingField;

public class SearchForEmptyCells
{
    public static void Get(CrosswordState gameState)
    {
        gameState.ListEmptyCellStruct.Clear();
        foreach (var cell in gameState.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Transparent)
            {
                gameState.ListEmptyCellStruct.Add(cell);
            }
        }
    }
}