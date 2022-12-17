using System.Collections.Generic;

namespace Crossword.Words;

public class AddWords
{
    public static void Get(Word word, List<string> listWordsList)
    {
        word.listWords = listWordsList;
        word.listTempWords = new List<string>(listWordsList);
    }
}