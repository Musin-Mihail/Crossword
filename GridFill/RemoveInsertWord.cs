﻿using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill;

public class RemoveInsertWord
{
    public static void Get(Word word)
    {
        int index = Global.allInsertedWords.IndexOf(word.wordString);
        if (index >= 0)
        {
            SearchDictionaryEntryRemove.Get(word.wordString);
            Global.allInsertedWords.RemoveAt(index);
        }

        word.wordString = "";
        word.full = false;
    }
}