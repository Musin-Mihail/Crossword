using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.Words;

public class SearchForMatches
{
    public static bool Get(Word word, Label matchLabel)
    {
        if (word.listLabel.Contains(matchLabel) == true)
        {
            return true;
        }

        return false;
    }
}