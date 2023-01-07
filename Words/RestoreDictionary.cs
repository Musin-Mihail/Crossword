﻿using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Words;

public class RestoreDictionary
{
    public static void Get(Word word)
    {
        word.listTempWords = new List<Dictionary>(Global.listDictionaries);
        ListWordsRandomization.Get(word);
    }
}