using System.Collections.Generic;
using System.Windows.Controls;

namespace Crossword.Words;

public class Word
{
    public List<Label> listLabel = new();
    public List<string> listWords = new();
    public List<string> listTempWords = new();
    public readonly List<Label> connectionLabel = new();
    public List<Word> connectionWords = new();
    public bool full = false;
    public string wordString = "";
    public List<string> allInsertedWords = new();
    public bool right = false;
    public int error = 0;
}