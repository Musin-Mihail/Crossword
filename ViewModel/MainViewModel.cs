using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Crossword.Main;
using Crossword.PlayingField;
using Crossword.SaveLoad;
using Crossword.Screenshot;
using Crossword.Services;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly CrosswordState _gameState;
    private readonly GenerationService _generationService;

    private string _statusMessage;
    private bool _isGenerating;
    private string _difficulty;
    private string _selectedDictionaryInfo;

    public ICommand StartGenerationCommand { get; }
    public ICommand StopGenerationCommand { get; }
    public ICommand SaveGridCommand { get; }
    public ICommand LoadGridCommand { get; }
    public ICommand ResetDictionariesCommand { get; }
    public ICommand SelectDictionariesCommand { get; }
    public ICommand CreateRequiredDictionaryCommand { get; }
    public ICommand ScreenshotCommand { get; }

    public MainViewModel(CrosswordState gameState, GenerationService generationService)
    {
        _gameState = gameState;
        _generationService = generationService;
        _statusMessage = _gameState.StatusMessage;
        _isGenerating = _gameState.IsGenerating;
        _difficulty = _gameState.Difficulty;
        _selectedDictionaryInfo = _gameState.SelectedDictionaryInfo;
        StartGenerationCommand = new RelayCommand(async _ => await StartGenerationAsync(), _ => !IsGenerating);
        StopGenerationCommand = new RelayCommand(_ => StopGeneration(), _ => IsGenerating);
        SaveGridCommand = new RelayCommand(_ => SaveGrid(), _ => !IsGenerating);
        LoadGridCommand = new RelayCommand(_ => LoadGrid(), _ => !IsGenerating);
        ResetDictionariesCommand = new RelayCommand(_ => ResetDictionaries(), _ => !IsGenerating);
        SelectDictionariesCommand = new RelayCommand(_ => SelectDictionaries(), _ => !IsGenerating);
        CreateRequiredDictionaryCommand = new RelayCommand(_ => CreateRequiredDictionary(), _ => !IsGenerating);
        ScreenshotCommand = new RelayCommand(_ => Screenshot(), _ => !IsGenerating);
        ResetDictionaries();
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            _gameState.StatusMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        private set
        {
            _isGenerating = value;
            _gameState.IsGenerating = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsUiEnabled));
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public bool IsUiEnabled => !IsGenerating;

    public string Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            _gameState.Difficulty = value;
            OnPropertyChanged();
        }
    }

    public string SelectedDictionaryInfo
    {
        get => _selectedDictionaryInfo;
        set
        {
            _selectedDictionaryInfo = value;
            _gameState.SelectedDictionaryInfo = value;
            OnPropertyChanged();
        }
    }

    public string MaxSecondsText { get; set; } = "2";
    public string TaskDelayText { get; set; } = "100";
    public bool IsVisualizationChecked { get; set; } = false;

    private async Task StartGenerationAsync()
    {
        if (IsGenerating) return;

        try
        {
            _gameState.MaxSeconds = int.Parse(MaxSecondsText);
            _gameState.TaskDelay = int.Parse(TaskDelayText);
        }
        catch
        {
            MessageBox.Show("ОШИБКА. Вводите только цифры");
            return;
        }

        SearchForEmptyCells.Get(_gameState);
        if (_gameState.ListEmptyCellStruct.Count == 0)
        {
            MessageBox.Show("На поле нет пустых ячеек для генерации.");
            return;
        }

        IsGenerating = true;
        try
        {
            _gameState.IsVisualizationEnabled = IsVisualizationChecked;
            UpdateStatus();
            await Task.Run(() => _generationService.GenerateAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка во время генерации:\n{ex.Message}");
        }
        finally
        {
            IsGenerating = false;
            UpdateStatus();
        }
    }

    private void StopGeneration()
    {
        _gameState.Stop = true;
    }

    private void SaveGrid()
    {
        SearchForEmptyCells.Get(_gameState);
        Save.Get(_gameState);
    }

    private void LoadGrid()
    {
        var loadGrid = new LoadGrid();
        loadGrid.ShowDialog();
        if (loadGrid.Ready)
        {
            Load.Get(loadGrid.ListEmptyCellStruct, _gameState);
        }
    }

    private void Screenshot()
    {
        if (_gameState.ListWordsGrid.Count > 0 && _gameState.ListEmptyCellStruct.Count > 1)
        {
            CreateImage.Get();
        }
        else
        {
            MessageBox.Show("Сетка не заполнена словами после генерации.");
        }
    }

    private void ResetDictionaries()
    {
        var resetDict = new ResetDict(_gameState);
        resetDict.Get();
        SelectedDictionaryInfo = _gameState.SelectedDictionaryInfo;
        MessageBox.Show("Выбран основной словарь");
    }

    private void SelectDictionaries()
    {
        var dictionariesSelection = new DictionariesSelection();
        dictionariesSelection.ShowDialog();
        if (dictionariesSelection.Ready)
        {
            _gameState.ListDictionaries.Clear();
            var message = "Выбранные словари:\n";
            var dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
            foreach (var selectedDictionaries in dictionariesSelection.SelectedDictionaries)
            {
                var list = new List<string>(selectedDictionaries.Split(';'));
                foreach (var path in dictionariesPaths)
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    if (list[0] == name)
                    {
                        message += selectedDictionaries + "\n";
                        var dictionary = CreateDictionary.Get(path);
                        dictionary.Name = name;
                        dictionary.MaxCount = int.Parse(list[1]);
                        _gameState.ListDictionaries.Add(dictionary);
                        break;
                    }
                }
            }

            var commonDictionary = CreateDictionary.Get("dict.txt");
            _gameState.ListDictionaries.Add(commonDictionary);
            _gameState.ListDictionaries[^1].Name = "Общий";
            _gameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
            MessageBox.Show(message);
            SelectedDictionaryInfo = message;
        }
    }

    private void CreateRequiredDictionary()
    {
        new RequiredDictionary().ShowDialog();
    }

    private async void UpdateStatus()
    {
        while (IsGenerating)
        {
            StatusMessage = _gameState.StatusMessage;
            Difficulty = _gameState.Difficulty;
            await Task.Delay(100);
        }

        StatusMessage = _gameState.StatusMessage;
        Difficulty = _gameState.Difficulty;
    }
}