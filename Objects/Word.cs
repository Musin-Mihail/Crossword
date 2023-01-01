﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace Crossword.Objects;

public class Word
{
    public List<Label> listLabel = new();
    public List<Dictionary> listWords = new();
    public List<Dictionary> listTempWords = new();
    public readonly List<Label> connectionLabel = new();
    public List<Word> connectionWords = new();
    public bool full = false;
    public string wordString = "";
    public List<string> allInsertedWords = new();
    public bool right = false;
    public int error = 0;
}