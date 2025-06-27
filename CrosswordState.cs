using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword;

public class CrosswordState
{
    public bool Stop = false;
    public readonly List<Cell> ListEmptyCellStruct = new();
    public readonly List<Cell> ListAllCellStruct = new();
    public readonly List<Dictionary> ListDictionaries = new();
    public readonly List<Word> ListWordsGrid = new();
    public readonly List<string> AllInsertedWords = new();
    public int Index = 0;
    public int MaxSeconds = 0;
    public int TaskDelay = 0;

    public bool IsGenerating { get; set; }
    public bool IsVisualizationEnabled { get; set; }
    public string StatusMessage { get; set; } = "Готов к генерации.";
    public string Difficulty { get; set; } = "Сложность: -";
    public string SelectedDictionaryInfo { get; set; } = "Основной словарь";
}