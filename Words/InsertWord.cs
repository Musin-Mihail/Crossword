using Crossword.Objects;

namespace Crossword.Words;

public class InsertWord
{
    public static void Get(Word word, string newWord)
    {
        for (int i = 0; i < word.listLabel.Count; i++)
        {
            word.listLabel[i].Content = newWord[i];
        }
    }
}