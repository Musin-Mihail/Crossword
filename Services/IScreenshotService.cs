using System.Collections.Generic;
using Crossword.Objects;
using Crossword.ViewModel;

namespace Crossword.Services
{
    public interface IScreenshotService
    {
        void CreateCrosswordFiles(List<Word> listWordsGrid, List<Dictionary> listDictionaries, IEnumerable<CellViewModel> cells);
    }
}