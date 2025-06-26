using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword;

public static class Global
{
    public static bool stop = false;
    public static readonly List<Cell> ListEmptyCellStruct = new();
    public static readonly List<Cell> ListAllCellStruct = new();
    public static readonly List<Dictionary> ListDictionaries = new();
    public static readonly List<Word> ListWordsGrid = new();
    public static readonly List<string> AllInsertedWords = new();
    public static int index = 0;
    public static int maxSeconds = 0;
    public static int taskDelay = 0;
    public static Label windowsText;
    public static Label difficultyLevel;
    public static Label selectedDictionary;
    public static CheckBox visualization;
    public static Grid gridGeneration;
    public static Button startGeneration;
    public static Button stopGeneration;
}