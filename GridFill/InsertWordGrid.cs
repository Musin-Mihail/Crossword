using System.Collections.Generic;
using Crossword.Words;

namespace Crossword.GridFill;

public class InsertWordGrid
{
    public static bool Get(List<string> allInsertedWords, Word word)
    {
        if (word.listTempWords.Count == 0)
        {
            return true;
        }

        bool error = SearchWord.Get(allInsertedWords, word.listLabel, word.listTempWords, word);
        if (error == true)
        {
            return true;
        }

        return false;
    }
}