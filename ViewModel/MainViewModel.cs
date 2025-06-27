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
    private string _statusMessage = App.GameState.StatusMessage;
    private bool _isGenerating = App.GameState.IsGenerating;
    private string _difficulty = App.GameState.Difficulty;
    private string _selectedDictionaryInfo = App.GameState.SelectedDictionaryInfo;

    public ICommand StartGenerationCommand { get; }
    public ICommand StopGenerationCommand { get; }
    public ICommand SaveGridCommand { get; }
    public ICommand LoadGridCommand { get; }
    public ICommand ResetDictionariesCommand { get; }
    public ICommand SelectDictionariesCommand { get; }
    public ICommand CreateRequiredDictionaryCommand { get; }
    public ICommand ScreenshotCommand { get; }

    public MainViewModel()
    {
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
            OnPropertyChanged();
        }
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        private set
        {
            _isGenerating = value;
            App.GameState.IsGenerating = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsUiEnabled));
        }
    }

    public bool IsUiEnabled => !IsGenerating;

    public string Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnPropertyChanged();
        }
    }

    public string SelectedDictionaryInfo
    {
        get => _selectedDictionaryInfo;
        set
        {
            _selectedDictionaryInfo = value;
            OnPropertyChanged();
        }
    }

    public string MaxSecondsText { get; set; } = "5";
    public string TaskDelayText { get; set; } = "10";
    public bool IsVisualizationChecked { get; set; } = false;

    private async Task StartGenerationAsync()
    {
        if (IsGenerating) return;

        try
        {
            App.GameState.MaxSeconds = int.Parse(MaxSecondsText);
            App.GameState.TaskDelay = int.Parse(TaskDelayText);
        }
        catch
        {
            MessageBox.Show("ОШИБКА. Вводите только цифры");
            return;
        }

        SearchForEmptyCells.Get();
        if (App.GameState.ListEmptyCellStruct.Count == 0)
        {
            MessageBox.Show("На поле нет пустых ячеек для генерации.");
            return;
        }

        IsGenerating = true;
        try
        {
            App.GameState.IsVisualizationEnabled = IsVisualizationChecked;
            var generationService = new GenerationService(App.GameState);
            UpdateStatus();

            await Task.Run(() => generationService.GenerateAsync());
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
        App.GameState.Stop = true;
    }

    private void SaveGrid()
    {
        SearchForEmptyCells.Get();
        Save.Get();
    }

    private void LoadGrid()
    {
        var loadGrid = new LoadGrid();
        loadGrid.ShowDialog();
        if (loadGrid.Ready)
        {
            Load.Get(loadGrid.ListEmptyCellStruct);
        }
    }

    private void Screenshot()
    {
        if (App.GameState.ListEmptyCellStruct.Count > 1)
        {
            CreateImage.Get();
        }
        else
        {
            MessageBox.Show("Ячеек меньше двух\nИли не было генерации");
        }
    }

    private void ResetDictionaries()
    {
        ResetDict.Get();
        SelectedDictionaryInfo = App.GameState.SelectedDictionaryInfo;
        MessageBox.Show("Выбран основной словарь");
    }

    private void SelectDictionaries()
    {
        var dictionariesSelection = new DictionariesSelection();
        dictionariesSelection.ShowDialog();
        if (dictionariesSelection.Ready)
        {
            App.GameState.ListDictionaries.Clear();
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
                        App.GameState.ListDictionaries.Add(dictionary);
                        break;
                    }
                }
            }

            var commonDictionary = CreateDictionary.Get("dict.txt");
            App.GameState.ListDictionaries.Add(commonDictionary);
            App.GameState.ListDictionaries[^1].Name = "Общий";
            App.GameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
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
            StatusMessage = App.GameState.StatusMessage;
            Difficulty = App.GameState.Difficulty;
            await Task.Delay(100);
        }

        StatusMessage = App.GameState.StatusMessage;
        Difficulty = App.GameState.Difficulty;
    }
}