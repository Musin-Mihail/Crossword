using Crossword.Objects;

namespace Crossword.FormationOfAQueue.SortingWords;

public class AddWordToList
{
    public static void Get(Word word)
    {
        foreach (var wordConnection in word.connectionWords)
        {
            if (GlobalSort.newList.Contains(wordConnection) == false)
            {
                GlobalSort.newList.Add(wordConnection);
                Get(wordConnection);
            }
        }
    }
}