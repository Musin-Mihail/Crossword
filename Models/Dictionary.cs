﻿using System.Collections.Generic;

namespace Crossword.Models;

public class Dictionary
{
    public string Name = "";
    public int MaxCount = 9999;
    public int CurrentCount = 0;
    public List<DictionaryWord> Words = new();
}