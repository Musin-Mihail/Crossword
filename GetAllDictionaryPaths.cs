using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crossword;

public class GetAllDictionaryPaths
{
    public static List<string> Get()
    {
        var dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
        return dictionariesPaths;
    }
}