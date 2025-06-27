using System.Windows.Media;
using Crossword.Words;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearAllCell
{
    public static void Get()
    {
        App.GameState.AllInsertedWords.Clear();
        foreach (var word in App.GameState.ListWordsGrid)
        {
            ResetWord.Get(word);
        }

        foreach (var cell in App.GameState.ListEmptyCellStruct)
        {
            cell.Label.Content = null;
            cell.Label.Background = Brushes.Transparent;
        }
    }
}