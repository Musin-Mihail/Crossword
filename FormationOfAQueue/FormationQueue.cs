namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static void Get(GenerationParameters genParams)
    {
        App.GameState.ListWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords.Get();
        SearchForConnectedWords.Get(genParams);
    }
}