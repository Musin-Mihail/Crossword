using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.Words;

public class ClearLabel
{
    public static void Get(Word word)
    {
        foreach (Label label in word.listLabel)
        {
            if (label.Content != null)
            {
                if (SearchConnectWord1.Get(word, label) == false)
                {
                    label.Content = null;
                }
                else if (SearchConnectWord2.Get(word, label) == true)
                {
                    label.Content = null;
                }
            }
        }
    }
}