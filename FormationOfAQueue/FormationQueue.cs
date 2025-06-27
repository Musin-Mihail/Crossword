namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static void Get()
    {
        App.GameState.ListWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords.Get();
        SearchForConnectedWords.Get();
    }
}