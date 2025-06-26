using System.Windows.Media;
using Crossword.Words;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearAllCell
{
    public static void Get()
    {
        Global.AllInsertedWords.Clear();
        foreach (var word in Global.ListWordsGrid)
        {
            ResetWord.Get(word);
        }

        foreach (var cell in Global.ListEmptyCellStruct)
        {
            cell.Label.Content = null;
            cell.Label.Background = Brushes.Transparent;
        }
    }
}