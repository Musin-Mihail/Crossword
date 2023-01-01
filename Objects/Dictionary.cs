using System.Collections.Generic;

namespace Crossword.Objects;

public class Dictionary
{
    public int maxCount = 9999;
    public int currentCount = 0;
    public List<DictionaryWord> words = new();

    public int Sort()
    {
        if (currentCount >= maxCount)
        {
            return 9999;
        }

        return currentCount;
    }
}