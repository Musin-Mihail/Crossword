using Crossword.Objects;

namespace Crossword.GridFill;

public class RemoveInsertWord
{
    public static void Get(Word word)
    {
        Global.allInsertedWords.RemoveAt(Global.allInsertedWords.IndexOf(word.wordString));
        word.wordString = "";
        word.full = false;
        foreach (var label in word.listLabel)
        {
            if (!word.connectionLabel.Contains(label))
            {
                label.Content = null;
            }
        }
    }
}