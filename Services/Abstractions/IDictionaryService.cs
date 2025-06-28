using System.Collections.Generic;
using Crossword.Models;

namespace Crossword.Services.Abstractions;

public interface IDictionaryService
{
    IEnumerable<string> GetDictionaryPaths();
    Dictionary LoadDictionary(string path);
}