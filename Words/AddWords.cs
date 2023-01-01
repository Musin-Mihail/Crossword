using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Words;

public class AddWords
{
    public static void Get(Word word)
    {
        word.listWords = new List<Dictionary>(Global.listDictionaries);
        word.listTempWords = new List<Dictionary>(Global.listDictionaries);
    }
}