using System.Collections.Generic;

namespace Crossword.Objects;

public class DictionaryWord
{
    public string Answers = "";
    public readonly List<string> Definitions = new();
}