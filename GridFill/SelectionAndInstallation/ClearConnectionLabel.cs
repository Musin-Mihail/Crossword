using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearConnectionLabel
{
    public static void Get(Word word)
    {
        foreach (var label in word.ListLabel)
        {
            if (!CheckConnectionLabel.Get(word, label))
            {
                label.Content = null;
            }
        }

        RemoveInsertWord.Get(word);
    }
}