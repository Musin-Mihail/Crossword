using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill;

public class TestWordStartGreen
{
    public static void Get(Word wordOld)
    {
        foreach (var item in wordOld.listLabel)
        {
            item.Background = Brushes.Green;
        }
    }
}