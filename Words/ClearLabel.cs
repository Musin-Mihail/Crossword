using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.Words;

public class ClearLabel
{
    public static void Get(Word word)
    {
        foreach (Label label in word.listLabel)
        {
            label.Content = null;
        }
    }
}