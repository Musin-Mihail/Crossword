using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue.SortingWords;

public class SortingWordsGridNew
{
    public static void Get()
    {
        GlobalSort.newList.Clear();
        GlobalSort.newList.Add(Global.listWordsGrid[0]);
        AddWordToList.Get(Global.listWordsGrid[0]);
        Global.listWordsGrid = new List<Word>(GlobalSort.newList);
    }
}