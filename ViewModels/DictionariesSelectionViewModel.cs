using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Crossword.Infrastructure;
using Crossword.Services.Abstractions;

namespace Crossword.ViewModels;

public class DictionariesSelectionViewModel
{
    public ObservableCollection<DictionaryInfoViewModel> Dictionaries { get; } = new();
    public List<string> SelectionResult { get; } = new();
    public ICommand AcceptCommand { get; }
    public event Action? CloseRequested;

    public DictionariesSelectionViewModel(IDictionaryService dictionaryService)
    {
        var dictionariesPaths = dictionaryService.GetDictionaryPaths();

        foreach (var path in dictionariesPaths)
        {
            var countWords = File.ReadAllLines(path).Length;
            var name = Path.GetFileNameWithoutExtension(path);
            Dictionaries.Add(new DictionaryInfoViewModel(name, countWords));
        }

        AcceptCommand = new RelayCommand(AcceptSelection);
    }

    private void AcceptSelection(object? parameter)
    {
        foreach (var dictInfo in Dictionaries)
        {
            if (!string.IsNullOrEmpty(dictInfo.SelectedWordCount) && int.TryParse(dictInfo.SelectedWordCount, out int count) && count > 0)
            {
                var finalCount = count > dictInfo.TotalWordCount ? dictInfo.TotalWordCount : count;
                SelectionResult.Add($"{dictInfo.Name};{finalCount}");
            }
        }

        CloseRequested?.Invoke();
    }
}