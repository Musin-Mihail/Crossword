using Crossword.Objects;

namespace Crossword.Words;

public class Reset
{
    public static void Get(Word word)
    {
        ClearLabel.Get(word);
        word.full = false;
        for (int i = 0; i < word.allInsertedWords.Count; i++)
        {
            if (word.allInsertedWords[i] == word.wordString)
            {
                word.allInsertedWords.RemoveAt(i);
                break;
            }
        }

        word.wordString = "";
    }
}