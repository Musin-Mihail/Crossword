using System.Collections.Generic;
using Crossword.Models;

namespace Crossword.Services.Abstractions;

public interface ICrosswordStateService
{
    List<Word> WordsGrid { get; }
    List<Dictionary> Dictionaries { get; }
    List<Cell> EmptyCells { get; }
}