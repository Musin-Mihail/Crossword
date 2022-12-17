using System.Collections.Generic;

namespace Crossword.Words;

public class RestoreDictionary
{
    public static void Get(Word word)
    {
        word.listTempWords = new List<string>(word.listWords);
        ListWordsRandomization.Get(word);
    }
}