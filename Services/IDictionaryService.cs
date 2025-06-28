using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Services;

public interface IDictionaryService
{
    IEnumerable<string> GetDictionaryPaths();
    Dictionary LoadDictionary(string path);
}