namespace Crossword.GridFill;

public class SearchDictionaryEntryRemove
{
    public static void Get(string answers)
    {
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers == answers)
                {
                    dictionary.CurrentCount--;
                    return;
                }
            }
        }
    }
}