using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword;

public class CrosswordState
{
    public bool Stop { get; set; }
    public List<Cell> ListEmptyCellStruct { get; } = new();
    public List<Cell> ListAllCellStruct { get; } = new();
    public List<Dictionary> ListDictionaries { get; } = new();
    public List<Word> ListWordsGrid { get; } = new();
    public List<string> AllInsertedWords { get; } = new();
    public int Index { get; set; }
    public int MaxSeconds { get; set; }
    public int TaskDelay { get; set; }
    public bool IsGenerating { get; set; }
    public bool IsVisualizationEnabled { get; set; }
    public string StatusMessage { get; set; } = "Готов к генерации.";
    public string Difficulty { get; set; } = "Сложность: -";
    public string SelectedDictionaryInfo { get; set; } = "Основной словарь";
}