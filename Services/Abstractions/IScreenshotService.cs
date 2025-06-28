using System.Collections.Generic;
using Crossword.Models;
using Crossword.ViewModels;

namespace Crossword.Services.Abstractions;

public interface IScreenshotService
{
    void ExportCrossword(List<Word> listWordsGrid, List<Dictionary> listDictionaries, IEnumerable<CellViewModel> cells);
}