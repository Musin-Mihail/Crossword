using System.Collections.Generic;

namespace Crossword.Models;

public class DictionaryWord
{
    public string Answers = "";
    public readonly List<string> Definitions = new();
}