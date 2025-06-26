using Crossword.Objects;

namespace Crossword.GridFill;

public class SearchDictionaryEntryAdd
{
    public static void Get(string answers, Word word)
    {
        foreach (var dictionary in Global.ListDictionaries)
        {
            if (dictionary.CurrentCount >= dictionary.MaxCount)
            {
                continue;
            }

            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers == answers)
                {
                    if (dictionary.Name == "!ОБЯЗАТЕЛЬНЫЕ")
                    {
                        word.Fix = true;
                    }

                    dictionary.CurrentCount++;
                    return;
                }
            }
        }
    }
}