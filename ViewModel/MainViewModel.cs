using System.Threading.Tasks;
using System.Windows;
using Crossword.Main;
using Crossword.PlayingField;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private string _statusMessage = App.GameState.StatusMessage;
    private bool _isGenerating = App.GameState.IsGenerating;
    private string _difficulty = App.GameState.Difficulty;
    private string _selectedDictionaryInfo = App.GameState.SelectedDictionaryInfo;

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    private bool IsGenerating
    {
        get => _isGenerating;
        set
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

    public MainViewModel()
    {
        ResetDictionaries();
    }

    public async Task StartGenerationAsync()
    {
        if (IsGenerating) return;
        IsGenerating = true;
        App.GameState.IsVisualizationEnabled = IsVisualizationChecked;
        SearchForEmptyCells.Get();
        if (App.GameState.ListEmptyCellStruct.Count == 0)
        {
            MessageBox.Show("На поле нет пустых ячеек для генерации.");
            IsGenerating = false;
            return;
        }

        UpdateStatus();
        await Task.Run(() => { Main.GridFill.Get(MaxSecondsText, TaskDelayText); });

        IsGenerating = false;
    }

    public void StopGeneration()
    {
        App.GameState.Stop = true;
    }

    public void ResetDictionaries()
    {
        ResetDict.Get();
        SelectedDictionaryInfo = App.GameState.SelectedDictionaryInfo;
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