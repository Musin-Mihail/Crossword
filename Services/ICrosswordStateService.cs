using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Services;

public interface ICrosswordStateService
{
    List<Word> WordsGrid { get; }
    List<Dictionary> Dictionaries { get; }
    List<Cell> EmptyCells { get; }
}