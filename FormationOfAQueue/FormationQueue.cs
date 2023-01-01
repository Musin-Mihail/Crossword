using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static List<Word> Get(List<Cell> listEmptyCellStruct)
    {
        List<Word> listWordStruct = new();
        SearchForTheBeginningAndLengthOfAllWords.Get(listWordStruct, listEmptyCellStruct);
        SearchForConnectedWords.Get(listWordStruct);
        listWordStruct = Sorting.Get(listWordStruct);
        SortingConnectionWords.Get(listWordStruct);
        return listWordStruct;
    }
}