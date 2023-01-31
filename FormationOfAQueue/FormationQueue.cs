namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static void Get()
    {
        Global.listWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords.Get();
        SearchForConnectedWords.Get();
    }
}