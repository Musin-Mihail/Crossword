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
using Crossword.Main;
using Crossword.Objects;
using Crossword.PlayingField;
using Crossword.SaveLoad;
using Crossword.Screenshot;
using Crossword.Services;

namespace Crossword.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly CrosswordState _gameState;
    private readonly GenerationService _generationService;
    private readonly IDialogService _dialogService;

    private string _statusMessage;
    private bool _isGenerating;
    private string _difficulty;
    private string _selectedDictionaryInfo;
    private bool _isVerticallyMirror;
    private bool _isHorizontallyMirror;
    private bool _isAllMirror;
    private bool _isVerticallyMirrorRevers;
    private bool _isHorizontallyMirrorRevers;
    private bool _isClearMirror = true;

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
    public ObservableCollection<CellViewModel> Cells { get; } = new();
    public ObservableCollection<HeaderViewModel> Headers { get; } = new();
    private int _numberOfCellsHorizontally = 30;
    private int _numberOfCellsVertically = 30;

    public MainViewModel(CrosswordState gameState, GenerationService generationService, IDialogService dialogService)
    {
        _gameState = gameState;
        _generationService = generationService;
        _dialogService = dialogService;

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
        ChangeFieldSizeCommand = new RelayCommand(_ => ChangeFieldSize());
        CellInteractionCommand = new RelayCommand(HandleCellInteraction, _ => !IsGenerating);

        _generationService.StatusUpdated += OnStatusUpdated;
        _generationService.VisualizeWordPlacement += OnVisualizeWordPlacementAsync;
        _generationService.ClearGridVisualization += OnClearGridVisualization;
        _generationService.GetGridPatternRequestHandler += OnGetGridPatternRequest;
        _generationService.SetWordRequestHandler += OnSetWordRequest;
        _generationService.ClearWordRequestHandler += OnClearWordRequest;

        CreatePlayingField();
        ResetDictionaries();
    }

    #region Service Event Handlers (адаптация к новым моделям)

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
        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var cellVm in Cells)
            {
                cellVm.Content = null;
            }
        });
    }

    private async Task OnVisualizeWordPlacementAsync(Word word, Brush color)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            var vmsToAnimate = new List<CellViewModel>();
            foreach (var cell in word.Cells)
            {
                var cellVm = FindCellVm(cell.X, cell.Y);
                if (cellVm != null)
                {
                    vmsToAnimate.Add(cellVm);
                    cellVm.Content = cell.Content;
                }
            }

            foreach (var vm in vmsToAnimate)
            {
                vm.Background = color;
            }

            await Task.Delay(_gameState.TaskDelay > 0 ? _gameState.TaskDelay : 1);

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
                var cellVm = FindCellVm(cell.X, cell.Y);
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
                var cellVm = FindCellVm(cell.X, cell.Y);
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
                    var cellVm = FindCellVm(cell.X, cell.Y);
                    if (cellVm != null)
                    {
                        cellVm.Content = null;
                    }
                }
            }
        });
    }

    private CellViewModel? FindCellVm(int x, int y) => Cells.FirstOrDefault(c => c.X == x && c.Y == y);

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
            }
        }
    }

    public bool IsUiEnabled => !IsGenerating;
    public int FieldWidth => (_numberOfCellsHorizontally + 1) * CellViewModel.CellSize;
    public int FieldHeight => (_numberOfCellsVertically + 1) * CellViewModel.CellSize;

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

    public bool IsClearMirror
    {
        get => _isClearMirror;
        set => SetProperty(ref _isClearMirror, value);
    }

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

    #region Grid Creation and Interaction Logic

    private void CreatePlayingField()
    {
        Cells.Clear();
        Headers.Clear();
        _gameState.ListAllCellStruct.Clear();

        for (var y = 1; y <= _numberOfCellsVertically; y++)
        {
            for (var x = 1; x <= _numberOfCellsHorizontally; x++)
            {
                var cellVm = new CellViewModel { X = x, Y = y };
                Cells.Add(cellVm);
                _gameState.ListAllCellStruct.Add(new Cell(x, y));
            }
        }

        for (var y = 0; y <= _numberOfCellsVertically; y++)
        {
            Headers.Add(new HeaderViewModel { Content = y == 0 ? "" : y.ToString(), X = 0, Y = y });
        }

        for (var x = 1; x <= _numberOfCellsHorizontally; x++)
        {
            Headers.Add(new HeaderViewModel { Content = x.ToString(), X = x, Y = 0 });
        }

        OnPropertyChanged(nameof(FieldWidth));
        OnPropertyChanged(nameof(FieldHeight));
        OnPropertyChanged(nameof(LineCenterH_X));
        OnPropertyChanged(nameof(LineCenterH_Y2));
        OnPropertyChanged(nameof(LineCenterV_Y));
        OnPropertyChanged(nameof(LineCenterV_X2));
    }

    private void HandleCellInteraction(object? param)
    {
        if (param is not CellViewModel cellVm) return;
        var newColor = (Mouse.LeftButton, Mouse.RightButton) switch
        {
            (MouseButtonState.Pressed, _) => Brushes.Transparent,
            (_, MouseButtonState.Pressed) => Brushes.Black,
            _ => cellVm.Background
        };
        if (newColor == cellVm.Background) return;

        if (IsVerticallyMirror) ColoringHorizontal(cellVm, newColor);
        else if (IsHorizontallyMirror) ColoringVertical(cellVm, newColor);
        else if (IsAllMirror) ColoringAll(cellVm, newColor);
        else if (IsVerticallyMirrorRevers) ColoringHorizontalRevers(cellVm, newColor);
        else if (IsHorizontallyMirrorRevers) ColoringVerticalRevers(cellVm, newColor);
        else cellVm.Background = newColor;
    }

    private void ColoringCell(int x, int y, Brush color)
    {
        var cellToColor = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
        if (cellToColor != null)
        {
            cellToColor.Background = color;
        }
    }

    private void ColoringVertical(CellViewModel cell, Brush c)
    {
        var center = _numberOfCellsHorizontally / 2;
        if (cell.X <= center)
        {
            cell.Background = c;
            var mX = _numberOfCellsHorizontally - cell.X + 1;
            ColoringCell(mX, cell.Y, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && cell.X == center + 1) cell.Background = c;
    }

    private void ColoringHorizontal(CellViewModel cell, Brush c)
    {
        var center = _numberOfCellsVertically / 2;
        if (cell.Y <= center)
        {
            cell.Background = c;
            var mY = _numberOfCellsVertically - cell.Y + 1;
            ColoringCell(cell.X, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0 && cell.Y == center + 1) cell.Background = c;
    }

    private void ColoringVerticalRevers(CellViewModel cell, Brush c)
    {
        var center = _numberOfCellsHorizontally / 2;
        if (cell.X <= center)
        {
            cell.Background = c;
            var mX = _numberOfCellsHorizontally - cell.X + 1;
            var mY = _numberOfCellsVertically - cell.Y + 1;
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && cell.X == center + 1) cell.Background = c;
    }

    private void ColoringHorizontalRevers(CellViewModel cell, Brush c)
    {
        var center = _numberOfCellsVertically / 2;
        if (cell.Y <= center)
        {
            cell.Background = c;
            var mX = _numberOfCellsHorizontally - cell.X + 1;
            var mY = _numberOfCellsVertically - cell.Y + 1;
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0 && cell.Y == center + 1) cell.Background = c;
    }

    private void ColoringAll(CellViewModel cell, Brush c)
    {
        var cH = _numberOfCellsHorizontally / 2;
        var cV = _numberOfCellsVertically / 2;
        if (cell.X <= cH && cell.Y <= cV)
        {
            cell.Background = c;
            var mX = _numberOfCellsHorizontally - cell.X + 1;
            var mY = _numberOfCellsVertically - cell.Y + 1;
            ColoringCell(mX, cell.Y, c);
            ColoringCell(cell.X, mY, c);
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && cell.X == cH + 1 && cell.Y <= cV)
        {
            cell.Background = c;
            var mY = _numberOfCellsVertically - cell.Y + 1;
            ColoringCell(cell.X, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0 && cell.X <= cH && cell.Y == cV + 1)
        {
            cell.Background = c;
            var mX = _numberOfCellsHorizontally - cell.X + 1;
            ColoringCell(mX, cell.Y, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && _numberOfCellsVertically % 2 != 0 && cell.X == cH + 1 && cell.Y == cV + 1) cell.Background = c;
    }

    private void ChangeFieldSize()
    {
        _dialogService.ShowChangeFillDialog(ref _numberOfCellsHorizontally, ref _numberOfCellsVertically);
        CreatePlayingField();
    }

    private void UpdateMirrorLines()
    {
        OnPropertyChanged(nameof(LineCenterVVisibility));
        OnPropertyChanged(nameof(LineCenterHVisibility));
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

        try
        {
            _gameState.MaxSeconds = int.Parse(MaxSecondsText);
            _gameState.TaskDelay = int.Parse(TaskDelayText);
        }
        catch
        {
            _dialogService.ShowMessage("ОШИБКА. Вводите только цифры");
            return;
        }

        _gameState.ListEmptyCellStruct.Clear();
        foreach (var cellVm in Cells.Where(c => c.Background == Brushes.Transparent))
        {
            var cellModel = _gameState.ListAllCellStruct.First(c => c.X == cellVm.X && c.Y == cellVm.Y);
            _gameState.ListEmptyCellStruct.Add(cellModel);
        }

        if (_gameState.ListEmptyCellStruct.Count == 0)
        {
            _dialogService.ShowMessage("На поле нет пустых ячеек для генерации.");
            return;
        }

        IsGenerating = true;
        try
        {
            _gameState.IsVisualizationEnabled = IsVisualizationChecked;
            await Task.Run(() => _generationService.GenerateAsync());
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessage($"Произошла ошибка во время генерации:\n{ex.Message}");
        }
        finally
        {
            IsGenerating = false;
            if (!string.IsNullOrWhiteSpace(_gameState.StatusMessage))
                StatusMessage = _gameState.StatusMessage;
        }
    }

    private void StopGeneration() => _gameState.Stop = true;

    private void SaveGrid()
    {
        _gameState.ListEmptyCellStruct.Clear();
        foreach (var cellVm in Cells.Where(c => c.Background == Brushes.Transparent))
        {
            var cellModel = _gameState.ListAllCellStruct.First(c => c.X == cellVm.X && c.Y == cellVm.Y);
            _gameState.ListEmptyCellStruct.Add(cellModel);
        }

        Save.Get(_gameState);
    }

    private void LoadGrid()
    {
        if (_dialogService.ShowLoadGridDialog(out var listEmptyCellStruct) == true && listEmptyCellStruct.Any())
        {
            foreach (var cellVm in Cells)
            {
                cellVm.Background = Brushes.Black;
                cellVm.Content = null;
            }

            _gameState.ListEmptyCellStruct.Clear();

            foreach (var item in listEmptyCellStruct)
            {
                var strings = item.Split(';');
                if (strings.Length == 2 && int.TryParse(strings[0], out var x) && int.TryParse(strings[1], out var y))
                {
                    var cellVm = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
                    if (cellVm != null)
                    {
                        cellVm.Background = Brushes.Transparent;
                    }

                    var cellModel = _gameState.ListAllCellStruct.FirstOrDefault(c => c.X == x && c.Y == y);
                    if (cellModel != null)
                    {
                        _gameState.ListEmptyCellStruct.Add(cellModel);
                    }
                }
            }
        }
    }

    private void Screenshot()
    {
        if (_gameState.ListWordsGrid.Count > 0 && Cells.Any(c => c.Background == Brushes.Transparent))
        {
            CreateImage.Get(_gameState, Cells);
        }
        else
        {
            _dialogService.ShowMessage("Сетка не заполнена словами после генерации.");
        }
    }

    private void ResetDictionaries()
    {
        var resetDict = new ResetDict(_gameState);
        resetDict.Get();
        SelectedDictionaryInfo = _gameState.SelectedDictionaryInfo;
    }

    private void SelectDictionaries()
    {
        if (_dialogService.ShowDictionariesSelectionDialog(out var selectedDictionaries) == true && selectedDictionaries.Any())
        {
            _gameState.ListDictionaries.Clear();
            var message = "Выбранные словари:\n";
            var dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
            foreach (var selectedDict in selectedDictionaries)
            {
                var list = new List<string>(selectedDict.Split(';'));
                var path = dictionariesPaths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p) == list[0]);
                if (path != null)
                {
                    message += selectedDict + "\n";
                    var dictionary = CreateDictionary.Get(path);
                    dictionary.Name = list[0];
                    dictionary.MaxCount = int.Parse(list[1]);
                    _gameState.ListDictionaries.Add(dictionary);
                }
            }

            var commonDictionary = CreateDictionary.Get("dict.txt");
            _gameState.ListDictionaries.Add(commonDictionary);
            _gameState.ListDictionaries[^1].Name = "Общий";
            _gameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
            _dialogService.ShowMessage(message);
            SelectedDictionaryInfo = message;
        }
    }

    private void CreateRequiredDictionary() => _dialogService.ShowRequiredDictionaryDialog();

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