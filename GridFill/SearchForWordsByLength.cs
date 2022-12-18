using System.Collections.Generic;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchForWordsByLength
{
    public static void Get(List<Word> listWordStruct, List<List<string>> listWordsList)
    {
        for (int i = 0; i < listWordStruct.Count; i++)
        {
            int letterCount = listWordStruct[i].listLabel.Count;
            AddWords.Get(listWordStruct[i], listWordsList[letterCount]);
        }

        foreach (var word in listWordStruct)
        {
            ListWordsRandomization.Get(word);
        }
    }
}