using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword;

public static class Global
{
    public static bool stop = false;
    
    public static List<Cell> listEmptyCellStruct = new();
    public static List<Cell> listAllCellStruct = new();
    public static List<Dictionary> listDictionaries = new();
    public static List<Word> listWordsGrid = new();
    public static List<string> allInsertedWords = new();
    
    public static int index = 0;
    public static int maxError = 50;
    public static int maxSeconds= 0;
    public static int taskDelay = 0;
    
    public static Label? windowsText;
    public static CheckBox? visualization;
    public static Grid? gridGeneration;
    public static Button? startGeneration;
    public static Button? stopGeneration;
}