using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class SortingWordsGrid
{
    public static void Get()
    {
        List<Word> tempList = new List<Word>();
        List<Word> newList = new List<Word>();
        List<Word> matchList = new List<Word>();
        foreach (var item in Global.listWordsGrid)
        {
            if (newList.Contains(item) == false)
            {
                newList.Add(item);
            }

            foreach (var item2 in item.connectionWords)
            {
                if (matchList.Contains(item2) == false)
                {
                    matchList.Add(item2);
                    tempList.Add(item2);
                }
            }

            while (tempList.Count > 0)
            {
                Word newWord = tempList[0];
                tempList.RemoveAt(0);
                if (newList.Contains(newWord) == false)
                {
                    newList.Add(newWord);
                }

                foreach (var item2 in newWord.connectionWords)
                {
                    if (matchList.Contains(item2) == false)
                    {
                        matchList.Add(item2);
                        tempList.Add(item2);
                    }
                }
            }
        }

        Global.listWordsGrid = new List<Word>(newList);
    }
}