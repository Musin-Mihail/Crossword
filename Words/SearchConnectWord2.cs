using System.Windows.Controls;

namespace Crossword.Words;

public class SearchConnectWord2
{
    public static bool Get(Word word1, Label label)
    {
        foreach (Word word in word1.connectionWords)
        {
            if (word.connectionLabel.Contains(label) == true)
            {
                if (word.full == false)
                {
                    return true;
                }
            }
        }

        return false;
    }
}