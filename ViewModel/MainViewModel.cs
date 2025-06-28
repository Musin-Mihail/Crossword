using System.Collections.ObjectModel;
using System.ComponentModel;
using Crossword.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly IGridManagerService _gridManagerService;
    private string _difficulty = "Сложность: -";
    public GenerationControlViewModel GenerationControls { get; }
    public GridControlViewModel GridControls { get; }
    public FileControlViewModel FileControls { get; }
    public DictionaryControlViewModel DictionaryControls { get; }
    public ObservableCollection<CellViewModel> Cells => _gridManagerService.Cells;
    public ObservableCollection<HeaderViewModel> Headers => _gridManagerService.Headers;

    public MainViewModel(IDialogService dialogService, IGridManagerService gridManagerService, IServiceScopeFactory scopeFactory)
    {
        _gridManagerService = gridManagerService;
        using (var scope = scopeFactory.CreateScope())
        {
            GenerationControls = scope.ServiceProvider.GetRequiredService<GenerationControlViewModel>();
            GridControls = new GridControlViewModel(dialogService, gridManagerService, () => !GenerationControls.IsGenerating);
            FileControls = scope.ServiceProvider.GetRequiredService<FileControlViewModel>();
            DictionaryControls = scope.ServiceProvider.GetRequiredService<DictionaryControlViewModel>();
        }

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