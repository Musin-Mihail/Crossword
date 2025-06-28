using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Services;

public class CrosswordStateService : ICrosswordStateService
{
    public List<Word> WordsGrid { get; } = new();
    public List<Dictionary> Dictionaries { get; } = new();
    public List<Cell> EmptyCells { get; } = new();
}