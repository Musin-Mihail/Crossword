using Crossword.Objects;

namespace Crossword.Words;

public class Reset
{
    public static void Get(Word word)
    {
        ClearLabel.Get(word);
        word.full = false;
        for (int i = 0; i < Global.allInsertedWords.Count; i++)
        {
            if (Global.allInsertedWords[i] == word.wordString)
            {
                Global.allInsertedWords.RemoveAt(i);
                break;
            }
        }

        word.wordString = "";
    }
}