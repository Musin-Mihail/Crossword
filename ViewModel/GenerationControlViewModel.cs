using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.Services;

namespace Crossword.ViewModel;

public class GenerationControlViewModel : ViewModelBase
{
    private readonly GenerationService _generationService;
    private readonly IDialogService _dialogService;
    private readonly IGridManagerService _gridManagerService;
    private readonly ICrosswordStateService _crosswordStateService;
    private string _statusMessage = "Готов к генерации.";
    private bool _isGenerating;
    public string MaxSecondsText { get; set; } = "2";
    public string TaskDelayText { get; set; } = "100";
    public bool IsVisualizationChecked { get; set; }
    public ICommand StartGenerationCommand { get; }
    public ICommand StopGenerationCommand { get; }

    public GenerationControlViewModel(
        GenerationService generationService,
        IDialogService dialogService,
        IGridManagerService gridManagerService,
        ICrosswordStateService crosswordStateService)
    {
        _generationService = generationService;
        _dialogService = dialogService;
        _gridManagerService = gridManagerService;
        _crosswordStateService = crosswordStateService;
        StartGenerationCommand = new RelayCommand(async _ => await StartGenerationAsync(), _ => !IsGenerating);
        StopGenerationCommand = new RelayCommand(_ => StopGeneration(), _ => IsGenerating);

        _generationService.StatusUpdated += OnStatusUpdated;
        _generationService.VisualizeWordPlacement += OnVisualizeWordPlacementAsync;
        _generationService.ClearGridVisualization += OnClearGridVisualization;
        _generationService.GetGridPatternRequestHandler += OnGetGridPatternRequest;
        _generationService.SetWordRequestHandler += OnSetWordRequest;
        _generationService.ClearWordRequestHandler += OnClearWordRequest;
    }

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
                ((RelayCommand)StartGenerationCommand).RaiseCanExecuteChanged();
                ((RelayCommand)StopGenerationCommand).RaiseCanExecuteChanged();
            }
        }
    }

    private async Task StartGenerationAsync()
    {
        if (IsGenerating) return;

        if (!int.TryParse(MaxSecondsText, out var maxSeconds))
        {
            _dialogService.ShowMessage("ОШИБКА. Вводите только цифры в поле 'Макс. секунд'.");
            return;
        }

        _crosswordStateService.EmptyCells.Clear();
        _crosswordStateService.EmptyCells.AddRange(_gridManagerService.GetEmptyCells());
        if (_crosswordStateService.EmptyCells.Count == 0)
        {
            _dialogService.ShowMessage("На поле нет пустых ячеек для генерации.");
            return;
        }

        _crosswordStateService.WordsGrid.Clear();
        IsGenerating = true;
        try
        {
            var result = await _generationService.GenerateAsync(_crosswordStateService.EmptyCells, _crosswordStateService.Dictionaries, maxSeconds, IsVisualizationChecked);
            _crosswordStateService.WordsGrid.AddRange(result);
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
}