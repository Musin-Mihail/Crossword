using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Crossword.Services;

namespace Crossword.ViewModel;

public class DictionaryControlViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IDictionaryService _dictionaryService;
    private readonly ICrosswordStateService _crosswordStateService;
    private string _selectedDictionaryInfo = "Основной словарь";
    public ICommand ResetDictionariesCommand { get; }
    public ICommand SelectDictionariesCommand { get; }
    public ICommand CreateRequiredDictionaryCommand { get; }

    public DictionaryControlViewModel(IDialogService dialogService, IDictionaryService dictionaryService, ICrosswordStateService crosswordStateService)
    {
        _dialogService = dialogService;
        _dictionaryService = dictionaryService;
        _crosswordStateService = crosswordStateService;

        ResetDictionariesCommand = new RelayCommand(_ => ResetDictionaries());
        SelectDictionariesCommand = new RelayCommand(_ => SelectDictionaries());
        CreateRequiredDictionaryCommand = new RelayCommand(_ => CreateRequiredDictionary());
        ResetDictionaries();
    }

    public string SelectedDictionaryInfo
    {
        get => _selectedDictionaryInfo;
        set => SetProperty(ref _selectedDictionaryInfo, value);
    }

    private void ResetDictionaries()
    {
        _crosswordStateService.Dictionaries.Clear();
        var commonDictionary = _dictionaryService.LoadDictionary("dict.txt");
        commonDictionary.Name = "Общий";
        commonDictionary.MaxCount = commonDictionary.Words.Count;
        _crosswordStateService.Dictionaries.Add(commonDictionary);
        SelectedDictionaryInfo = "Основной словарь";
    }

    private void SelectDictionaries()
    {
        if (_dialogService.ShowDictionariesSelectionDialog(out var selectedDictionaries) == true && selectedDictionaries.Any())
        {
            _crosswordStateService.Dictionaries.Clear();
            var message = "Выбранные словари:\n";
            var dictionariesPaths = _dictionaryService.GetDictionaryPaths().ToList();
            foreach (var selectedDict in selectedDictionaries)
            {
                var list = new List<string>(selectedDict.Split(';'));
                var path = dictionariesPaths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p) == list[0]);
                if (path != null)
                {
                    message += selectedDict + "\n";
                    var dictionary = _dictionaryService.LoadDictionary(path);
                    dictionary.Name = list[0];
                    dictionary.MaxCount = int.Parse(list[1]);
                    _crosswordStateService.Dictionaries.Add(dictionary);
                }
            }

            var commonDictionary = _dictionaryService.LoadDictionary("dict.txt");
            commonDictionary.Name = "Общий";
            commonDictionary.MaxCount = commonDictionary.Words.Count;
            _crosswordStateService.Dictionaries.Add(commonDictionary);
            _dialogService.ShowMessage(message);
            SelectedDictionaryInfo = message;
        }
    }

    private void CreateRequiredDictionary()
    {
        _dialogService.ShowRequiredDictionaryDialog(_crosswordStateService.Dictionaries);
    }
}