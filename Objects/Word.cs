﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace Crossword.Objects;

public class Word
{
    public List<Label> listLabel = new();
    public readonly List<Label> connectionLabel = new();
    public List<Word> connectionWords = new();
    public bool full = false;
    public string wordString = "";
    public bool right = false;
    public List<Dictionary> fullDictionaries = new();
    public bool fix = false;
}