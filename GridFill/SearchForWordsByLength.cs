using System.Collections.Generic;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchForWordsByLength
{
    public static void Get(List<Word> listWordStruct, List<Dictionary> listWordsList)
    {
        for (int i = 0; i < listWordStruct.Count; i++)
        {
            AddWords.Get(listWordStruct[i], listWordsList);
        }

        foreach (Word word in listWordStruct)
        {
            ListWordsRandomization.Get(word);
        }
    }
}