using System.Collections.Generic;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchForWordsByLength
{
    public static void Get()
    {
        for (int i = 0; i < Global.listWordsGrid.Count; i++)
        {
            Word word = Global.listWordsGrid[i];
            AddWords.Get(word);
        }

        foreach (Word word in Global.listWordsGrid)
        {
            ListWordsRandomization.Get(word);
        }
    }
}