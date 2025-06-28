using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Crossword.Objects;
using Crossword.Services;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly IGridManagerService _gridManagerService;
    private readonly List<Cell> _listEmptyCellStruct = new();
    private readonly List<Dictionary> _listDictionaries = new();
    private readonly List<Word> _listWordsGrid = new();
    private string _difficulty = "Сложность: -";
    public GenerationControlViewModel GenerationControls { get; }
    public GridControlViewModel GridControls { get; }
    public FileControlViewModel FileControls { get; }
    public DictionaryControlViewModel DictionaryControls { get; }
    public ObservableCollection<CellViewModel> Cells => _gridManagerService.Cells;
    public ObservableCollection<HeaderViewModel> Headers => _gridManagerService.Headers;

    public MainViewModel(GenerationService generationService, IDialogService dialogService, IDictionaryService dictionaryService, IScreenshotService screenshotService, IGridManagerService gridManagerService)
    {
        _gridManagerService = gridManagerService;
        GenerationControls = new GenerationControlViewModel(generationService, dialogService, gridManagerService, _listEmptyCellStruct, _listDictionaries, _listWordsGrid);
        GridControls = new GridControlViewModel(dialogService, gridManagerService, () => !GenerationControls.IsGenerating);
        FileControls = new FileControlViewModel(dialogService, screenshotService, gridManagerService, _listWordsGrid, _listDictionaries, _listEmptyCellStruct);
        DictionaryControls = new DictionaryControlViewModel(dialogService, dictionaryService, _listDictionaries);
        GenerationControls.PropertyChanged += OnGenerationControlsPropertyChanged;
    }

    private void OnGenerationControlsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GenerationControls.IsGenerating))
        {
            OnPropertyChanged(nameof(IsUiEnabled));
            ((RelayCommand)GridControls.CellInteractionCommand).RaiseCanExecuteChanged();
            ((RelayCommand)FileControls.SaveGridCommand).RaiseCanExecuteChanged();
            ((RelayCommand)FileControls.LoadGridCommand).RaiseCanExecuteChanged();
            ((RelayCommand)FileControls.ScreenshotCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DictionaryControls.SelectDictionariesCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DictionaryControls.ResetDictionariesCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DictionaryControls.CreateRequiredDictionaryCommand).RaiseCanExecuteChanged();
        }
    }

    public bool IsUiEnabled => !GenerationControls.IsGenerating;

    public string Difficulty
    {
        get => _difficulty;
        set => SetProperty(ref _difficulty, value);
    }

    public class HeaderViewModel
    {
        public string Content { get; set; } = "";
        public int X { get; set; }
        public int Y { get; set; }
        public int DisplayX => X * CellViewModel.CellSize;
        public int DisplayY => Y * CellViewModel.CellSize;
        public int Width => CellViewModel.CellSize;
        public int Height => CellViewModel.CellSize;
    }
}