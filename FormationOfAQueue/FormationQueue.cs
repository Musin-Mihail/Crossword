namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static void Get()
    {
        Global.ListWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords.Get();
        SearchForConnectedWords.Get();
    }
}