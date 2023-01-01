using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.GridFill;

public class InsertWordGrid
{
    public static bool Get(List<string> allInsertedWords, Word word)
    {
        if (word.listTempWords.Count == 0)
        {
            return true;
        }

        string answer = SearchWord.Get(allInsertedWords, word);
        if (answer == "")
        {
            return true;
        }

        for (int i = 0; i < word.listLabel.Count; i++)
        {
            word.listLabel[i].Content = answer[i];
        }

        word.full = true;
        word.wordString = answer;

        return false;
    }
}