using System.Collections.Generic;
using System.Linq;
using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class SortingConnectionWords
{
    public static void Get(List<Word> listWordStruct)
    {
        foreach (var item in listWordStruct)
        {
            item.connectionWords = item.connectionWords.OrderByDescending(word => (float)word.connectionLabel.Count / word.listLabel.Count).ToList();
        }
    }
}