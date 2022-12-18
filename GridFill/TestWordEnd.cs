using System.Windows.Media;
using Crossword.Words;

namespace Crossword.GridFill;

public class TestWordEnd
{
    public static void Get(Word word)
    {
        foreach (var item in word.listLabel)
        {
            item.Background = Brushes.Transparent;
        }
    }
}