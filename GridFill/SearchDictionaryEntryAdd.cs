using Crossword.Objects;

namespace Crossword.GridFill;

public class SearchDictionaryEntryAdd
{
    public static void Get(string answers, Word word)
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
                    if (dictionary.name == "!ОБЯЗАТЕЛЬНЫЕ")
                    {
                        word.fix = true;
                    }

                    dictionary.currentCount++;
                    return;
                }
            }
        }
    }
}