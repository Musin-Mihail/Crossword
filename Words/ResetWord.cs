﻿using Crossword.Objects;

namespace Crossword.Words;

public class ResetWord
{
    public static void Get(Word word)
    {
        ClearLabel.Get(word);
        word.full = false;
        word.wordString = "";
        word.fix = false;
    }
}