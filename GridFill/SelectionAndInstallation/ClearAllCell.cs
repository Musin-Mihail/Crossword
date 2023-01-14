using System.Windows.Media;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearAllCell
{
    public static void Get()
    {
        Global.allInsertedWords.Clear();
        foreach (Word word in Global.listWordsGrid)
        {
            ResetWord.Get(word);
        }

        foreach (Cell cell in Global.listEmptyCellStruct)
        {
            cell.label.Content = null;
            cell.label.Background = Brushes.Transparent;
        }
    }
}