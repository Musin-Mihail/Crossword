using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword;

public static class Global
{
    public static bool stop = false;
    public static List<Cell> listEmptyCellStruct = new();
    public static List<Cell> listAllCellStruct = new();
    public static List<Dictionary> listDictionaries = new();
    public static List<Word> listWordsGrid = new();
}