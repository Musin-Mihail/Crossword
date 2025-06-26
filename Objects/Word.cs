using System.Collections.Generic;
using System.Windows.Controls;

namespace Crossword.Objects;

public class Word
{
    public List<Label> ListLabel = new();
    public readonly List<Label> ConnectionLabel = new();
    public readonly List<Word> ConnectionWords = new();
    public bool Full = false;
    public string WordString = "";
    public bool Right = false;
    public readonly List<Dictionary> FullDictionaries = new();
    public bool Fix = false;
}