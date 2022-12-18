using System.Collections.Generic;
using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class Sorting
{
    public static List<Word> Get(List<Word> listWordStruct)
    {
        List<Word> tempList = new List<Word>();
        List<Word> NewList = new List<Word>();
        List<Word> matchList = new List<Word>();
        foreach (var item in listWordStruct)
        {
            if (NewList.Contains(item) == false)
            {
                NewList.Add(item);
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
                if (NewList.Contains(newWord) == false)
                {
                    NewList.Add(newWord);
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

        return NewList;
    }
}