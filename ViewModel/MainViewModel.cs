using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.Services;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly GenerationService _generationService;
    private readonly IDialogService _dialogService;
    private readonly IDictionaryService _dictionaryService;
    private readonly IScreenshotService _screenshotService;
    private readonly IGridManagerService _gridManagerService;

    private List<Cell> _listEmptyCellStruct = new();
    private readonly List<Dictionary> _listDictionaries = new();
    private readonly List<Word> _listWordsGrid = new();

    private string _statusMessage = "Готов к генерации.";
    private bool _isGenerating;
    private string _difficulty = "Сложность: -";
    private string _selectedDictionaryInfo = "Основной словарь";

    private int _numberOfCellsHorizontally = 30;
    private int _numberOfCellsVertically = 30;

    private bool _isVerticallyMirror;
    private bool _isHorizontallyMirror;
    private bool _isAllMirror;
    private bool _isVerticallyMirrorRevers;
    private bool _isHorizontallyMirrorRevers;

    public ICommand StartGenerationCommand { get; }
    public ICommand StopGenerationCommand { get; }
    public ICommand SaveGridCommand { get; }
    public ICommand LoadGridCommand { get; }
    public ICommand ResetDictionariesCommand { get; }
    public ICommand SelectDictionariesCommand { get; }
    public ICommand CreateRequiredDictionaryCommand { get; }
    public ICommand ScreenshotCommand { get; }
    public ICommand ChangeFieldSizeCommand { get; }
    public ICommand CellInteractionCommand { get; }

    public ObservableCollection<CellViewModel> Cells => _gridManagerService.Cells;
    public ObservableCollection<HeaderViewModel> Headers => _gridManagerService.Headers;

    public MainViewModel(GenerationService generationService, IDialogService dialogService, IDictionaryService dictionaryService, IScreenshotService screenshotService, IGridManagerService gridManagerService)
    {
        _generationService = generationService;
        _dialogService = dialogService;
        _dictionaryService = dictionaryService;
        _screenshotService = screenshotService;
        _gridManagerService = gridManagerService;

        StartGenerationCommand = new RelayCommand(async _ => await StartGenerationAsync(), _ => !IsGenerating);
        StopGenerationCommand = new RelayCommand(_ => StopGeneration(), _ => IsGenerating);
        SaveGridCommand = new RelayCommand(_ => SaveGrid(), _ => !IsGenerating);
        LoadGridCommand = new RelayCommand(_ => LoadGrid(), _ => !IsGenerating);
        ResetDictionariesCommand = new RelayCommand(_ => ResetDictionaries(), _ => !IsGenerating);
        SelectDictionariesCommand = new RelayCommand(_ => SelectDictionaries(), _ => !IsGenerating);
        CreateRequiredDictionaryCommand = new RelayCommand(_ => CreateRequiredDictionary(), _ => !IsGenerating);
        ScreenshotCommand = new RelayCommand(_ => Screenshot(), _ => !IsGenerating);
        ChangeFieldSizeCommand = new RelayCommand(_ => ChangeFieldSize());
        CellInteractionCommand = new RelayCommand(HandleCellInteraction, _ => !IsGenerating);

        _generationService.StatusUpdated += OnStatusUpdated;
        _generationService.VisualizeWordPlacement += OnVisualizeWordPlacementAsync;
        _generationService.ClearGridVisualization += OnClearGridVisualization;
        _generationService.GetGridPatternRequestHandler += OnGetGridPatternRequest;
        _generationService.SetWordRequestHandler += OnSetWordRequest;
        _generationService.ClearWordRequestHandler += OnClearWordRequest;

        _gridManagerService.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(IGridManagerService.FieldWidth)) OnPropertyChanged(nameof(FieldWidth));
            if (e.PropertyName == nameof(IGridManagerService.FieldHeight)) OnPropertyChanged(nameof(FieldHeight));
        };

        ResetDictionaries();
        UpdateMirrorLines();
    }

    #region Service Event Handlers

    private void OnStatusUpdated(string status)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (status.StartsWith("ГЕНЕРАЦИЯ УДАЛАСЬ"))
            {
                _dialogService.ShowMessage(status);
                StatusMessage = "Генерация завершена.";
            }
            else
            {
                StatusMessage = status;
            }
        });
    }

    private void OnClearGridVisualization()
    {
        Application.Current.Dispatcher.Invoke(() => { _gridManagerService.ClearAllCellsContent(); });
    }

    private async Task OnVisualizeWordPlacementAsync(Word word, Brush color)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            var vmsToAnimate = new List<CellViewModel>();
            foreach (var cell in word.Cells)
            {
                var cellVm = _gridManagerService.FindCellVm(cell.X, cell.Y);
                if (cellVm != null)
                {
                    vmsToAnimate.Add(cellVm);
                    cellVm.Content = cell.Content;
                }
            }

            if (!int.TryParse(TaskDelayText, out var taskDelay) || taskDelay <= 0)
            {
                taskDelay = 1;
            }

            foreach (var vm in vmsToAnimate)
            {
                vm.Background = color;
            }

            await Task.Delay(taskDelay);

            foreach (var vm in vmsToAnimate)
            {
                vm.Background = Brushes.Transparent;
            }
        });
    }

    private string OnGetGridPatternRequest(Word word)
    {
        return Application.Current.Dispatcher.Invoke(() =>
        {
            var pattern = "";
            foreach (var cell in word.Cells)
            {
                var cellVm = _gridManagerService.FindCellVm(cell.X, cell.Y);
                pattern += cellVm?.Content ?? "*";
            }

            return pattern;
        });
    }

    private void OnSetWordRequest(Word word, string answer)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            for (var i = 0; i < word.Cells.Count; i++)
            {
                var cell = word.Cells[i];
                cell.Content = answer[i].ToString();
                var cellVm = _gridManagerService.FindCellVm(cell.X, cell.Y);
                if (cellVm != null)
                {
                    cellVm.Content = cell.Content;
                }
            }
        });
    }

    private void OnClearWordRequest(Word word)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var cell in word.Cells)
            {
                var isIntersection = word.ConnectionWords.Any(connectedWord =>
                    connectedWord.Full &&
                    connectedWord.Cells.Any(c => c.X == cell.X && c.Y == cell.Y));

                if (!isIntersection)
                {
                    cell.Content = null;
                    var cellVm = _gridManagerService.FindCellVm(cell.X, cell.Y);
                    if (cellVm != null)
                    {
                        cellVm.Content = null;
                    }
                }
            }
        });
    }

    #endregion

    #region Properties for Binding

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        private set
        {
            if (SetProperty(ref _isGenerating, value))
            {
                OnPropertyChanged(nameof(IsUiEnabled));
                ((RelayCommand)StartGenerationCommand).RaiseCanExecuteChanged();
                ((RelayCommand)StopGenerationCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SaveGridCommand).RaiseCanExecuteChanged();
                ((RelayCommand)LoadGridCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ResetDictionariesCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SelectDictionariesCommand).RaiseCanExecuteChanged();
                ((RelayCommand)CreateRequiredDictionaryCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ScreenshotCommand).RaiseCanExecuteChanged();
                ((RelayCommand)CellInteractionCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsUiEnabled => !IsGenerating;

    public int FieldWidth => _gridManagerService.FieldWidth;
    public int FieldHeight => _gridManagerService.FieldHeight;

    public string Difficulty
    {
        get => _difficulty;
        set => SetProperty(ref _difficulty, value);
    }

    public string SelectedDictionaryInfo
    {
        get => _selectedDictionaryInfo;
        set => SetProperty(ref _selectedDictionaryInfo, value);
    }

    public string MaxSecondsText { get; set; } = "2";
    public string TaskDelayText { get; set; } = "100";
    public bool IsVisualizationChecked { get; set; }

    public bool IsVerticallyMirror
    {
        get => _isVerticallyMirror;
        set => SetProperty(ref _isVerticallyMirror, value);
    }

    public bool IsHorizontallyMirror
    {
        get => _isHorizontallyMirror;
        set => SetProperty(ref _isHorizontallyMirror, value);
    }

    public bool IsAllMirror
    {
        get => _isAllMirror;
        set => SetProperty(ref _isAllMirror, value);
    }

    public bool IsVerticallyMirrorRevers
    {
        get => _isVerticallyMirrorRevers;
        set => SetProperty(ref _isVerticallyMirrorRevers, value);
    }

    public bool IsHorizontallyMirrorRevers
    {
        get => _isHorizontallyMirrorRevers;
        set => SetProperty(ref _isHorizontallyMirrorRevers, value);
    }

    public Visibility LineCenterVVisibility => (IsVerticallyMirror || IsVerticallyMirrorRevers || IsAllMirror) ? Visibility.Visible : Visibility.Hidden;
    public Visibility LineCenterHVisibility => (IsHorizontallyMirror || IsHorizontallyMirrorRevers || IsAllMirror) ? Visibility.Visible : Visibility.Hidden;
    public double LineCenterH_X => (_numberOfCellsHorizontally * CellViewModel.CellSize / 2.0) + CellViewModel.CellSize;
    public double LineCenterH_Y2 => (_numberOfCellsVertically + 1) * CellViewModel.CellSize;
    public double LineCenterV_Y => (_numberOfCellsVertically * CellViewModel.CellSize / 2.0) + CellViewModel.CellSize;
    public double LineCenterV_X2 => (_numberOfCellsHorizontally + 1) * CellViewModel.CellSize;

    #endregion

    #region Grid Interaction Logic

    private void HandleCellInteraction(object? param)
    {
        if (param is not CellViewModel cellVm) return;

        _gridManagerService.SetMirrorModes(IsVerticallyMirror, IsHorizontallyMirror, IsAllMirror, IsVerticallyMirrorRevers, IsHorizontallyMirrorRevers);
        _gridManagerService.HandleCellInteraction(cellVm);
    }

    private void ChangeFieldSize()
    {
        _dialogService.ShowChangeFillDialog(ref _numberOfCellsHorizontally, ref _numberOfCellsVertically);
        _gridManagerService.InitializeGrid(_numberOfCellsHorizontally, _numberOfCellsVertically);
        UpdateMirrorLines(); // Обновляем вспомогательные линии
    }

    private void UpdateMirrorLines()
    {
        OnPropertyChanged(nameof(LineCenterVVisibility));
        OnPropertyChanged(nameof(LineCenterHVisibility));
        OnPropertyChanged(nameof(LineCenterH_X));
        OnPropertyChanged(nameof(LineCenterH_Y2));
        OnPropertyChanged(nameof(LineCenterV_Y));
        OnPropertyChanged(nameof(LineCenterV_X2));
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName?.Contains("Mirror") ?? false)
        {
            UpdateMirrorLines();
        }
    }

    #endregion

    #region Commands Methods

    private async Task StartGenerationAsync()
    {
        if (IsGenerating) return;

        if (!int.TryParse(MaxSecondsText, out var maxSeconds))
        {
            _dialogService.ShowMessage("ОШИБКА. Вводите только цифры в поле 'Макс. секунд'.");
            return;
        }

        _listEmptyCellStruct = _gridManagerService.GetEmptyCells();

        if (_listEmptyCellStruct.Count == 0)
        {
            _dialogService.ShowMessage("На поле нет пустых ячеек для генерации.");
            return;
        }

        _listWordsGrid.Clear();
        IsGenerating = true;
        try
        {
            var result = await _generationService.GenerateAsync(_listEmptyCellStruct, _listDictionaries, maxSeconds, IsVisualizationChecked);
            _listWordsGrid.AddRange(result);
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessage($"Произошла ошибка во время генерации:\n{ex.Message}");
        }
        finally
        {
            IsGenerating = false;
        }
    }

    private void StopGeneration() => _generationService.RequestStop();

    private void SaveGrid()
    {
        var listEmptyCellStruct = _gridManagerService.GetEmptyCells();
        var saveFile = "";
        foreach (var cell in listEmptyCellStruct)
        {
            saveFile += $"{cell.X};{cell.Y}\n";
        }

        var name = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
        try
        {
            Directory.CreateDirectory("SaveGrid");
            File.WriteAllText(@"SaveGrid\" + name + ".grid", saveFile);
            _dialogService.ShowMessage("Сетка сохранена");
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessage($"Ошибка сохранения сетки: {ex.Message}");
        }
    }

    private void LoadGrid()
    {
        if (_dialogService.ShowLoadGridDialog(out var listEmptyCellStruct) == true && listEmptyCellStruct.Any())
        {
            _listEmptyCellStruct = _gridManagerService.LoadGridFromStruct(listEmptyCellStruct);
        }
    }

    private void Screenshot()
    {
        if (_listWordsGrid.Any() && Cells.Any(c => c.Background == Brushes.Transparent))
        {
            _screenshotService.CreateCrosswordFiles(_listWordsGrid, _listDictionaries, Cells);
        }
        else
        {
            _dialogService.ShowMessage("Сетка не заполнена словами после генерации.");
        }
    }

    private void ResetDictionaries()
    {
        _listDictionaries.Clear();
        var commonDictionary = _dictionaryService.LoadDictionary("dict.txt");
        _listDictionaries.Add(commonDictionary);
        _listDictionaries[^1].Name = "Общий";
        _listDictionaries[^1].MaxCount = commonDictionary.Words.Count;
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
            _listDictionaries.Add(commonDictionary);
            _listDictionaries[^1].Name = "Общий";
            _listDictionaries[^1].MaxCount = commonDictionary.Words.Count;
            _dialogService.ShowMessage(message);
            SelectedDictionaryInfo = message;
        }
    }

    private void CreateRequiredDictionary()
    {
        _dialogService.ShowRequiredDictionaryDialog(_listDictionaries);
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

    #endregion
}