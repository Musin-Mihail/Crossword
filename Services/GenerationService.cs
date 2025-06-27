using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.Services;

public class GenerationService
{
    private readonly CrosswordState _state;

    public GenerationService(CrosswordState state)
    {
        _state = state;
    }

    public async Task GenerateAsync()
    {
        _state.Stop = false;
        _state.AllInsertedWords.Clear();
        FormationQueue();
        _state.Difficulty = "Сложность - " + CalculateDifficultyLevel();
        await GenerateWords();
    }

    #region Word Queue Formation (from FormationOfAQueue namespace)

    private void FormationQueue()
    {
        _state.ListWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords();
        SearchForConnectedWords();
    }

    private void SearchForTheBeginningAndLengthOfAllWords()
    {
        foreach (var cell in _state.ListEmptyCellStruct)
        {
            var x = cell.X;
            var y = cell.Y;
            var isStartOfHorizontal = true;
            foreach (var cell2 in _state.ListEmptyCellStruct)
            {
                if (cell2.X == x - 1 && cell2.Y == y)
                {
                    isStartOfHorizontal = false;
                    break;
                }
            }

            if (isStartOfHorizontal) SaveWordRight(x, y);
            var isStartOfVertical = true;
            foreach (var cell2 in _state.ListEmptyCellStruct)
            {
                if (cell2.X == x && cell2.Y == y - 1)
                {
                    isStartOfVertical = false;
                    break;
                }
            }

            if (isStartOfVertical) SaveWordDown(x, y);
        }
    }

    private void SaveWordRight(int x, int y)
    {
        var newListLabel = new List<Label>();
        for (var i = x; i < 31; i++)
        {
            var match = false;
            foreach (var cell in _state.ListEmptyCellStruct)
            {
                if (cell.Y == y && cell.X == i)
                {
                    newListLabel.Add(cell.Label);
                    match = true;
                    break;
                }
            }

            if (!match) break;
        }

        if (newListLabel.Count > 1)
        {
            _state.ListWordsGrid.Add(new Word { ListLabel = newListLabel, Right = true });
        }
    }

    private void SaveWordDown(int x, int y)
    {
        var newListLabel = new List<Label>();
        for (var i = y; i < 31; i++)
        {
            var match = false;
            foreach (var cell in _state.ListEmptyCellStruct)
            {
                if (cell.Y == i && cell.X == x)
                {
                    newListLabel.Add(cell.Label);
                    match = true;
                    break;
                }
            }

            if (!match) break;
        }

        if (newListLabel.Count > 1)
        {
            _state.ListWordsGrid.Add(new Word { ListLabel = newListLabel });
        }
    }

    private void SearchForConnectedWords()
    {
        foreach (var word in _state.ListWordsGrid)
        {
            if (_state.Stop) return;
            CreateWordSpecificDictionaries(word);
            foreach (var label in word.ListLabel)
            {
                foreach (var word2 in _state.ListWordsGrid)
                {
                    if (word != word2 && word2.ListLabel.Contains(label))
                    {
                        if (!word.ConnectionWords.Contains(word2)) word.ConnectionWords.Add(word2);
                        if (!word.ConnectionLabel.Contains(label)) word.ConnectionLabel.Add(label);
                    }
                }
            }
        }
    }

    #endregion

    #region Generation Logic (from GridFill and other namespaces)

    private async Task GenerateWords()
    {
        var maxIndex = 0;
        var startDate = DateTime.Now;
        var singleAttemptDate = DateTime.Now;
        RestartGenerationAttempt();
        while (_state.Index < _state.ListWordsGrid.Count)
        {
            if ((DateTime.Now - singleAttemptDate).TotalSeconds > _state.MaxSeconds)
            {
                maxIndex = 0;
                singleAttemptDate = DateTime.Now;
                RestartGenerationAttempt();
                continue;
            }

            if (_state.Index > maxIndex)
            {
                singleAttemptDate = DateTime.Now;
                maxIndex = _state.Index;
                _state.StatusMessage = $"Подобрано {_state.Index} из {_state.ListWordsGrid.Count}";
                await Task.Delay(1);
            }

            if (_state.Stop)
            {
                _state.Stop = false;
                _state.StatusMessage = "Генерация остановлена пользователем.";
                return;
            }

            var currentWord = _state.ListWordsGrid[_state.Index];
            if (currentWord.Full)
            {
                _state.Index++;
                continue;
            }

            if (TryInsertWordIntoGrid(currentWord))
            {
                if (_state.IsVisualizationEnabled)
                {
                    await VisualizeWordPlacement(currentWord, Brushes.Green);
                }

                _state.Index++;
                continue;
            }

            if (_state.IsVisualizationEnabled)
            {
                await VisualizeWordPlacement(currentWord, Brushes.Red);
            }

            await StepBack(currentWord);
        }

        if (_state.Index >= _state.ListWordsGrid.Count)
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

        Application.Current.Dispatcher.Invoke(() =>
        {
            for (var i = 0; i < word.ListLabel.Count; i++)
            {
                word.ListLabel[i].Content = answer[i];
            }
        });
        word.Full = true;
        word.WordString = answer;
        return true;
    }

    private string FindMatchingWord(Word word)
    {
        if (word.ListLabel.Count <= 1) return "";
        var gridPattern = "";
        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var label in word.ListLabel)
            {
                gridPattern += label.Content?.ToString() ?? "*";
            }
        });
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

                if (matches && !_state.AllInsertedWords.Contains(dictionaryWord.Answers))
                {
                    _state.AllInsertedWords.Add(dictionaryWord.Answers);
                    AddDictionaryEntry(dictionaryWord.Answers, word);
                    return dictionaryWord.Answers;
                }
            }
        }

        return "";
    }

    private async Task StepBack(Word currentWord)
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
                    if (_state.IsVisualizationEnabled)
                    {
                        await VisualizeWordPlacement(currentWord, Brushes.Green);
                    }

                    _state.Index = 0;
                    return;
                }
            }
        }

        _state.Index = 0;
    }

    private void ClearWordFromGrid(Word word)
    {
        foreach (var label in word.ListLabel)
        {
            var isIntersection = false;
            foreach (var connectedWord in word.ConnectionWords)
            {
                if (connectedWord.Full && connectedWord.ListLabel.Contains(label))
                {
                    isIntersection = true;
                    break;
                }
            }

            if (!isIntersection)
            {
                Application.Current.Dispatcher.Invoke(() => label.Content = null);
            }
        }

        RemoveWord(word);
    }

    private void RemoveWord(Word word)
    {
        if (!string.IsNullOrEmpty(word.WordString))
        {
            RemoveDictionaryEntry(word.WordString);
            _state.AllInsertedWords.Remove(word.WordString);
        }

        word.WordString = "";
        word.Full = false;
    }

    private void RestartGenerationAttempt()
    {
        _state.Index = 0;
        Application.Current.Dispatcher.Invoke(ClearAllCells);
        foreach (var dictionary in _state.ListDictionaries)
        {
            dictionary.CurrentCount = 0;
        }

        var rnd = new Random();
        for (var i = _state.ListWordsGrid.Count - 1; i > 0; i--)
        {
            var j = rnd.Next(i + 1);
            (_state.ListWordsGrid[i], _state.ListWordsGrid[j]) = (_state.ListWordsGrid[j], _state.ListWordsGrid[i]);
        }
    }

    private void ClearAllCells()
    {
        _state.AllInsertedWords.Clear();
        foreach (var word in _state.ListWordsGrid)
        {
            word.Full = false;
            word.WordString = "";
            word.Fix = false;
            foreach (var label in word.ListLabel)
            {
                label.Content = null;
            }
        }

        foreach (var cell in _state.ListEmptyCellStruct)
        {
            cell.Label.Background = Brushes.Transparent;
        }
    }

    private void HandleSuccessfulGeneration(DateTime startDate)
    {
        var time = DateTime.Now - startDate;
        _state.StatusMessage = "ГЕНЕРАЦИЯ УДАЛАСЬ";
        var message = "";
        foreach (var dictionary in _state.ListDictionaries)
        {
            message += $"\n{dictionary.Name} - {dictionary.CurrentCount}/{dictionary.MaxCount}";
            dictionary.CurrentCount = 0;
        }

        Application.Current.Dispatcher.Invoke(() =>
            MessageBox.Show($"ГЕНЕРАЦИЯ УДАЛАСЬ\nза {time.TotalSeconds:F2} секунд\n{message}")
        );
    }

    #endregion

    #region Helpers

    private async Task VisualizeWordPlacement(Word word, Brush color)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            foreach (var item in word.ListLabel) item.Background = color;
            await Task.Delay(_state.TaskDelay);
            foreach (var item in word.ListLabel) item.Background = Brushes.Transparent;
        });
    }

    private float CalculateDifficultyLevel()
    {
        if (_state.ListWordsGrid.Count == 0) return 0;
        float difficultyLevel = 0;
        foreach (var word in _state.ListWordsGrid)
        {
            if (word.ListLabel.Count > 0)
            {
                difficultyLevel += (float)word.ConnectionLabel.Count / word.ListLabel.Count;
            }
        }

        return difficultyLevel / _state.ListWordsGrid.Count;
    }

    private void CreateWordSpecificDictionaries(Word word)
    {
        var hasWords = false;
        word.FullDictionaries.Clear();
        foreach (var dictionary in _state.ListDictionaries)
        {
            var matchingWords = new List<DictionaryWord>();
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.Length == word.ListLabel.Count)
                {
                    matchingWords.Add(dictionaryWord);
                }
            }

            var newDictionary = new Dictionary
            {
                Name = dictionary.Name,
                Words = matchingWords
            };
            if (newDictionary.Words.Count > 0) hasWords = true;
            word.FullDictionaries.Add(newDictionary);
        }

        if (!hasWords && _state.ListEmptyCellStruct.Count > 0)
        {
            _state.Stop = true;
            throw new Exception($"Для слова длиной {word.ListLabel.Count} нет подходящих слов в словарях.");
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
        foreach (var dictionary in _state.ListDictionaries)
        {
            if (dictionary.Name == nameDictionary)
            {
                return dictionary.CurrentCount < dictionary.MaxCount;
            }
        }

        return true;
    }

    private void AddDictionaryEntry(string answer, Word word)
    {
        foreach (var dictionary in _state.ListDictionaries)
        {
            if (dictionary.CurrentCount >= dictionary.MaxCount) continue;
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers == answer)
                {
                    if (dictionary.Name == "!ОБЯЗАТЕЛЬНЫЕ") word.Fix = true;
                    dictionary.CurrentCount++;
                    return;
                }
            }
        }
    }

    private void RemoveDictionaryEntry(string answer)
    {
        foreach (var dictionary in _state.ListDictionaries)
        {
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers == answer)
                {
                    dictionary.CurrentCount--;
                    return;
                }
            }
        }
    }

    #endregion
}