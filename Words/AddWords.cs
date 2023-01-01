using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Words;

public class AddWords
{
    public static void Get(Word word, List<Dictionary> listWordsList)
    {
        word.listWords = new List<Dictionary>(listWordsList);
        word.listTempWords = new List<Dictionary>(listWordsList);
    }
}