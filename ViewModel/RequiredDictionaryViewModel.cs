using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crossword.Objects;
using Crossword.Services;

namespace Crossword.ViewModel;

public class RequiredDictionaryViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private List<Dictionary> _availableDictionaries = new();

    private string _wordsText = "";

    public string WordsText
    {
        get => _wordsText;
        set => SetProperty(ref _wordsText, value);
    }

    public RelayCommand CreateDictionaryCommand { get; }

    public event EventHandler? CloseRequested;

    public RequiredDictionaryViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        CreateDictionaryCommand = new RelayCommand(CreateDictionary, _ => !string.IsNullOrWhiteSpace(WordsText));
    }

    public void Initialize(List<Dictionary> availableDictionaries)
    {
        _availableDictionaries = availableDictionaries;
    }

    private void CreateDictionary(object? parameter)
    {
        var words = WordsText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<DictionaryWord> dictionaryWords = new();

        foreach (var word in words)
        {
            if (!SearchMatch(word, dictionaryWords))
            {
                _dialogService.ShowMessage(word + ". Нет совпадений\nСловарь не сформирован");
                return;
            }
        }

        SaveFile(dictionaryWords);
        _dialogService.ShowMessage("Словарь сформирован\n!ОБЯЗАТЕЛЬНЫЕ.txt");
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private bool SearchMatch(string word, List<DictionaryWord> dictionaryWords)
    {
        foreach (var dictionary in _availableDictionaries)
        {
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    dictionaryWords.Add(dictionaryWord);
                    return true;
                }
            }
        }

        return false;
    }

    private void SaveFile(List<DictionaryWord> dictionaryWords)
    {
        var newDictionary = new List<string>();
        foreach (var dictionaryWord in dictionaryWords)
        {
            var line = dictionaryWord.Answers + ";" + string.Join(";", dictionaryWord.Definitions);
            newDictionary.Add(line);
        }

        const string path = "Dictionaries/!ОБЯЗАТЕЛЬНЫЕ.txt";
        Directory.CreateDirectory("Dictionaries");
        File.WriteAllLines(path, newDictionary);
    }
}