using System.Collections.Generic;

namespace Crossword.Objects;

public class Dictionary
{
    public string name = "";
    public int maxCount = 9999;
    public int currentCount = 0;
    public List<DictionaryWord> words = new();
}