using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Models;

namespace Crossword.Services;

public class GenerationService
{
    private List<Cell> _emptyCellModels = new();
    private List<Dictionary> _dictionaries = new();
    private List<Word> _wordGrid = new();
    private List<string> _allInsertedWords = new();
    private bool _stop;
    private int _maxSeconds;
    private int _index;

    public event Action<string>? StatusUpdated;
    public event Func<Word, Brush, Task>? VisualizeWordPlacement;
    public event Action? ClearGridVisualization;
    public event Func<Word, string>? GetGridPatternRequestHandler;
    public event Action<Word, string>? SetWordRequestHandler;
    public event Action<Word>? ClearWordRequestHandler;

    public async Task<List<Word>> GenerateAsync(List<Cell> emptyCells, List<Dictionary> dictionaries, int maxSeconds, bool isVisualizationEnabled)
    {
        _emptyCellModels = emptyCells;
        _dictionaries = dictionaries;
        _maxSeconds = maxSeconds;
        _stop = false;
        _allInsertedWords.Clear();
        _wordGrid.Clear();

        InitializeWordGrid();
        if (_stop)
        {
            StatusUpdated?.Invoke("Ошибка: не удалось сформировать очередь слов.");
            return new List<Word>();
        }

        StatusUpdated?.Invoke("Сложность - " + CalculateDifficultyLevel());
        await GenerateWords(isVisualizationEnabled);

        return _wordGrid;
    }

    public void RequestStop()
    {
        _stop = true;
    }

    #region Word Queue Formation

    private void InitializeWordGrid()
    {
        FindAllWordSpans();
        SearchForConnectedWords();
    }

    private void FindAllWordSpans()
    {
        foreach (var cell in _emptyCellModels)
        {
            var x = cell.X;
            var y = cell.Y;
            var isStartOfHorizontal = !_emptyCellModels.Any(cell2 => cell2.X == x - 1 && cell2.Y == y);
            if (isStartOfHorizontal) SaveWordRight(x, y);
            var isStartOfVertical = !_emptyCellModels.Any(cell2 => cell2.X == x && cell2.Y == y - 1);
            if (isStartOfVertical) SaveWordDown(x, y);
        }
    }

    private void SaveWordRight(int x, int y)
    {
        var newCells = new List<Cell>();
        for (var i = x; i < 31; i++)
        {
            var cell = _emptyCellModels.FirstOrDefault(c => c.Y == y && c.X == i);
            if (cell != null) newCells.Add(cell);
            else break;
        }

        if (newCells.Count > 1)
        {
            var newWord = new Word { Right = true };
            newWord.Cells.AddRange(newCells);
            _wordGrid.Add(newWord);
        }
    }

    private void SaveWordDown(int x, int y)
    {
        var newCells = new List<Cell>();
        for (var i = y; i < 31; i++)
        {
            var cell = _emptyCellModels.FirstOrDefault(c => c.Y == i && c.X == x);
            if (cell != null) newCells.Add(cell);
            else break;
        }

        if (newCells.Count > 1)
        {
            var newWord = new Word { Right = false };
            newWord.Cells.AddRange(newCells);
            _wordGrid.Add(newWord);
        }
    }

    private void SearchForConnectedWords()
    {
        foreach (var word in _wordGrid)
        {
            if (_stop) return;
            CreateWordSpecificDictionaries(word);
            foreach (var cell in word.Cells)
            {
                foreach (var word2 in _wordGrid)
                {
                    if (word != word2 && word2.Cells.Any(c => c.X == cell.X && c.Y == cell.Y))
                    {
                        if (!word.ConnectionWords.Contains(word2)) word.ConnectionWords.Add(word2);
                        var connectionCell = word2.Cells.First(c => c.X == cell.X && c.Y == cell.Y);
                        if (!word.ConnectionCells.Contains(connectionCell)) word.ConnectionCells.Add(connectionCell);
                    }
                }
            }
        }
    }

    #endregion

    #region Generation Logic

    private async Task GenerateWords(bool isVisualizationEnabled)
    {
        var maxIndex = 0;
        var startDate = DateTime.Now;
        var singleAttemptDate = DateTime.Now;
        RestartGenerationAttempt();
        while (_index < _wordGrid.Count)
        {
            if ((DateTime.Now - singleAttemptDate).TotalSeconds > _maxSeconds)
            {
                maxIndex = 0;
                singleAttemptDate = DateTime.Now;
                RestartGenerationAttempt();
                continue;
            }

            if (_index > maxIndex)
            {
                singleAttemptDate = DateTime.Now;
                maxIndex = _index;
                StatusUpdated?.Invoke($"Подобрано {_index} из {_wordGrid.Count}");
                await Task.Delay(1);
            }

            if (_stop)
            {
                StatusUpdated?.Invoke("Генерация остановлена пользователем.");
                return;
            }

            var currentWord = _wordGrid[_index];
            if (currentWord.Full)
            {
                _index++;
                continue;
            }

            if (TryInsertWordIntoGrid(currentWord))
            {
                if (isVisualizationEnabled && VisualizeWordPlacement != null)
                {
                    await VisualizeWordPlacement.Invoke(currentWord, Brushes.Green);
                }

                _index++;
                continue;
            }

            if (isVisualizationEnabled && VisualizeWordPlacement != null)
            {
                await VisualizeWordPlacement.Invoke(currentWord, Brushes.Red);
            }

            await StepBack(currentWord, isVisualizationEnabled);
        }

        if (_index >= _wordGrid.Count)
        {
            HandleSuccessfulGeneration(startDate);
        }
    }

    private bool TryInsertWordIntoGrid(Word word)
    {
        var answer = FindMatchingWord(word);
        if (string.IsNullOrEmpty(answer))
        {
            return false;
        }

        SetWordRequestHandler?.Invoke(word, answer);
        word.Full = true;
        word.WordString = answer;
        return true;
    }

    private string FindMatchingWord(Word word)
    {
        if (word.Cells.Count <= 1) return "";
        var gridPattern = GetGridPatternRequestHandler?.Invoke(word);
        if (gridPattern == null) return "";
        RandomizeWordLists(word);
        foreach (var dictionary in word.FullDictionaries)
        {
            if (!IsDictionaryAvailable(dictionary.Name)) continue;
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.Length != gridPattern.Length) continue;
                var matches = true;
                for (var i = 0; i < gridPattern.Length; i++)
                {
                    if (gridPattern[i] != '*' && gridPattern[i] != dictionaryWord.Answers[i])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches && !_allInsertedWords.Contains(dictionaryWord.Answers))
                {
                    _allInsertedWords.Add(dictionaryWord.Answers);
                    AddDictionaryEntry(dictionaryWord.Answers, word);
                    return dictionaryWord.Answers;
                }
            }
        }

        return "";
    }

    private async Task StepBack(Word currentWord, bool isVisualizationEnabled)
    {
        var random = new Random();
        var connectedWords = new List<Word>(currentWord.ConnectionWords);
        for (var i = connectedWords.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (connectedWords[i], connectedWords[j]) = (connectedWords[j], connectedWords[i]);
        }

        foreach (var wordToClear in connectedWords)
        {
            if (wordToClear is { Full: true, Fix: false })
            {
                ClearWordFromGrid(wordToClear);
                if (TryInsertWordIntoGrid(currentWord))
                {
                    if (isVisualizationEnabled && VisualizeWordPlacement != null)
                    {
                        await VisualizeWordPlacement.Invoke(currentWord, Brushes.Green);
                    }

                    _index = 0;
                    return;
                }
            }
        }

        _index = 0;
    }

    private void ClearWordFromGrid(Word word)
    {
        ClearWordRequestHandler?.Invoke(word);
        RemoveWord(word);
    }

    private void RemoveWord(Word word)
    {
        if (!string.IsNullOrEmpty(word.WordString))
        {
            RemoveDictionaryEntry(word.WordString);
            _allInsertedWords.Remove(word.WordString);
        }

        word.WordString = "";
        word.Full = false;
    }

    private void RestartGenerationAttempt()
    {
        _index = 0;
        _allInsertedWords.Clear();
        foreach (var word in _wordGrid)
        {
            word.Full = false;
            word.WordString = "";
            word.Fix = false;
            foreach (var cell in word.Cells)
            {
                cell.Content = null;
            }
        }

        ClearGridVisualization?.Invoke();
        foreach (var dictionary in _dictionaries)
        {
            dictionary.CurrentCount = 0;
        }

        var rnd = new Random();
        for (var i = _wordGrid.Count - 1; i > 0; i--)
        {
            var j = rnd.Next(i + 1);
            (_wordGrid[i], _wordGrid[j]) = (_wordGrid[j], _wordGrid[i]);
        }
    }

    private void HandleSuccessfulGeneration(DateTime startDate)
    {
        var time = DateTime.Now - startDate;
        var message = "";
        foreach (var dictionary in _dictionaries)
        {
            message += $"\n{dictionary.Name} - {dictionary.CurrentCount}/{dictionary.MaxCount}";
            dictionary.CurrentCount = 0;
        }

        StatusUpdated?.Invoke($"ГЕНЕРАЦИЯ УДАЛАСЬ\nза {time.TotalSeconds:F2} секунд\n{message}");
    }

    #endregion

    #region Helpers

    private float CalculateDifficultyLevel()
    {
        if (_wordGrid.Count == 0) return 0;
        float difficultyLevel = 0;
        foreach (var word in _wordGrid)
        {
            if (word.Cells.Count > 0)
            {
                difficultyLevel += (float)word.ConnectionCells.Count / word.Cells.Count;
            }
        }

        return difficultyLevel / _wordGrid.Count;
    }

    private void CreateWordSpecificDictionaries(Word word)
    {
        var hasWords = false;
        word.FullDictionaries.Clear();
        foreach (var dictionary in _dictionaries)
        {
            var matchingWords = new List<DictionaryWord>();
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.Length == word.Cells.Count)
                {
                    matchingWords.Add(dictionaryWord);
                }
            }

            if (matchingWords.Count > 0)
            {
                hasWords = true;
                word.FullDictionaries.Add(new Dictionary { Name = dictionary.Name, Words = matchingWords });
            }
        }

        if (!hasWords && _emptyCellModels.Count > 0)
        {
            _stop = true;
            StatusUpdated?.Invoke($"Для слова длиной {word.Cells.Count} нет подходящих слов в словарях.");
        }
    }

    private void RandomizeWordLists(Word word)
    {
        var rnd = new Random();
        foreach (var dictionary in word.FullDictionaries)
        {
            for (var i = dictionary.Words.Count - 1; i > 0; i--)
            {
                var j = rnd.Next(i + 1);
                (dictionary.Words[i], dictionary.Words[j]) = (dictionary.Words[j], dictionary.Words[i]);
            }
        }
    }

    private bool IsDictionaryAvailable(string nameDictionary)
    {
        var dictionary = _dictionaries.FirstOrDefault(d => d.Name == nameDictionary);
        return dictionary == null || dictionary.CurrentCount < dictionary.MaxCount;
    }

    private void AddDictionaryEntry(string answer, Word word)
    {
        foreach (var dictionary in _dictionaries.Where(d => d.CurrentCount < d.MaxCount))
        {
            if (dictionary.Words.Any(w => w.Answers == answer))
            {
                if (dictionary.Name == "!ОБЯЗАТЕЛЬНЫЕ") word.Fix = true;
                dictionary.CurrentCount++;
                return;
            }
        }
    }

    private void RemoveDictionaryEntry(string answer)
    {
        foreach (var dictionary in _dictionaries)
        {
            if (dictionary.Words.Any(w => w.Answers == answer))
            {
                dictionary.CurrentCount--;
                return;
            }
        }
    }

    #endregion
}