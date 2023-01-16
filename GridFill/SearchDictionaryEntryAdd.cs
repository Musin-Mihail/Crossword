namespace Crossword.GridFill;

public class SearchDictionaryEntryAdd
{
    public static void Get(string answers)
    {
        foreach (var dictionary in Global.listDictionaries)
        {
            if (dictionary.currentCount >= dictionary.maxCount)
            {
                continue;
            }

            foreach (var dictionaryWord in dictionary.words)
            {
                if (dictionaryWord.answers == answers)
                {
                    dictionary.currentCount++;
                    return;
                }
            }
        }
    }
}