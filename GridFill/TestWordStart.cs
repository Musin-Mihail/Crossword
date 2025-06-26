using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill;

public class TestWordStart
{
    public static void Get(Word wordOld, SolidColorBrush color)
    {
        foreach (var item in wordOld.ListLabel)
        {
            item.Background = color;
        }
    }
}