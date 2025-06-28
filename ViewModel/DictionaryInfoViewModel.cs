using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Crossword.ViewModel;

public class DictionaryInfoViewModel : INotifyPropertyChanged
{
    private string _selectedWordCount = "0";

    public string Name { get; set; }
    public int TotalWordCount { get; set; }

    public string SelectedWordCount
    {
        get => _selectedWordCount;
        set
        {
            if (value.All(char.IsDigit))
            {
                _selectedWordCount = value;
                OnPropertyChanged();
            }
        }
    }

    public DictionaryInfoViewModel(string name, int totalWordCount)
    {
        Name = name;
        TotalWordCount = totalWordCount;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class DictionariesSelectionViewModel
{
    public ObservableCollection<DictionaryInfoViewModel> Dictionaries { get; } = new();
    public List<string> SelectionResult { get; } = new();
    public ICommand AcceptCommand { get; }
    public event Action? CloseRequested;

    public DictionariesSelectionViewModel()
    {
        var dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
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