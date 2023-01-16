namespace Crossword.GridFill;

public class SearchDictionaryEntryRemove
{
    public static void Get(string answers)
    {
        foreach (var dictionary in Global.listDictionaries)
        {
            foreach (var dictionaryWord in dictionary.words)
            {
                if (dictionaryWord.answers == answers)
                {
                    dictionary.currentCount--;
                    return;
                }
            }
        }
    }
}