using Crossword.Objects;

namespace Crossword.Words;

public class ClearLabel
{
    public static void Get(Word word)
    {
        foreach (var label in word.ListLabel)
        {
            label.Content = null;
        }
    }
}