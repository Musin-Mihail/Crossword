using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Infrastructure;
using Crossword.Services.Abstractions;

namespace Crossword.ViewModels;

public class FileControlViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IScreenshotService _screenshotService;
    private readonly IGridManagerService _gridManagerService;
    private readonly ICrosswordStateService _crosswordStateService;
    public ICommand SaveGridCommand { get; }
    public ICommand LoadGridCommand { get; }
    public ICommand ScreenshotCommand { get; }

    public FileControlViewModel(
        IDialogService dialogService,
        IScreenshotService screenshotService,
        IGridManagerService gridManagerService,
        ICrosswordStateService crosswordStateService)
    {
        _dialogService = dialogService;
        _screenshotService = screenshotService;
        _gridManagerService = gridManagerService;
        _crosswordStateService = crosswordStateService;
        SaveGridCommand = new RelayCommand(_ => SaveGrid());
        LoadGridCommand = new RelayCommand(_ => LoadGrid());
        ScreenshotCommand = new RelayCommand(_ => Screenshot());
    }

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
            _crosswordStateService.EmptyCells.Clear();
            _crosswordStateService.EmptyCells.AddRange(_gridManagerService.LoadGridFromStruct(listEmptyCellStruct));
        }
    }

    private void Screenshot()
    {
        if (_crosswordStateService.WordsGrid.Any() && _gridManagerService.Cells.Any(c => c.Background == Brushes.Transparent))
        {
            _screenshotService.ExportCrossword(_crosswordStateService.WordsGrid, _crosswordStateService.Dictionaries, _gridManagerService.Cells);
        }
        else
        {
            _dialogService.ShowMessage("Сетка не заполнена словами после генерации.");
        }
    }
}