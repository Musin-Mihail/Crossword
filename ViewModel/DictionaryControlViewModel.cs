using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Crossword.Objects;
using Crossword.Services;

namespace Crossword.ViewModel;

public class DictionaryControlViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IDictionaryService _dictionaryService;
    private readonly List<Dictionary> _listDictionaries;
    private string _selectedDictionaryInfo = "Основной словарь";
    public ICommand ResetDictionariesCommand { get; }
    public ICommand SelectDictionariesCommand { get; }
    public ICommand CreateRequiredDictionaryCommand { get; }

    public DictionaryControlViewModel(IDialogService dialogService, IDictionaryService dictionaryService, List<Dictionary> listDictionaries)
    {
        _dialogService = dialogService;
        _dictionaryService = dictionaryService;
        _listDictionaries = listDictionaries;
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
        _listDictionaries.Clear();
        var commonDictionary = _dictionaryService.LoadDictionary("dict.txt");
        commonDictionary.Name = "Общий";
        commonDictionary.MaxCount = commonDictionary.Words.Count;
        _listDictionaries.Add(commonDictionary);
        SelectedDictionaryInfo = "Основной словарь";
    }

    private void SelectDictionaries()
    {
        if (_dialogService.ShowDictionariesSelectionDialog(out var selectedDictionaries) == true && selectedDictionaries.Any())
        {
            _listDictionaries.Clear();
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
                    _listDictionaries.Add(dictionary);
                }
            }

            var commonDictionary = _dictionaryService.LoadDictionary("dict.txt");
            commonDictionary.Name = "Общий";
            commonDictionary.MaxCount = commonDictionary.Words.Count;
            _listDictionaries.Add(commonDictionary);
            _dialogService.ShowMessage(message);
            SelectedDictionaryInfo = message;
        }
    }

    private void CreateRequiredDictionary()
    {
        _dialogService.ShowRequiredDictionaryDialog(_listDictionaries);
    }
}