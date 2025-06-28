using System.Collections.Generic;

namespace Crossword.Objects;

public class Word
{
    public List<Cell> Cells { get; } = new();
    public List<Cell> ConnectionCells { get; } = new();
    public List<Word> ConnectionWords { get; } = new();
    public bool Full { get; set; }
    public string WordString { get; set; } = "";
    public bool Right { get; set; }
    public List<Dictionary> FullDictionaries { get; } = new();
    public bool Fix { get; set; } = false;
}