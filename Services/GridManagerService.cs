using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.ViewModel;

namespace Crossword.Services;

public class GridManagerService : ViewModelBase, IGridManagerService
{
    private int _numberOfCellsHorizontally = 30;
    private int _numberOfCellsVertically = 30;

    private readonly List<Cell> _listAllCellStruct = new();

    private bool _isVerticallyMirror;
    private bool _isHorizontallyMirror;
    private bool _isAllMirror;
    private bool _isVerticallyMirrorRevers;
    private bool _isHorizontallyMirrorRevers;

    public ObservableCollection<CellViewModel> Cells { get; } = new();
    public ObservableCollection<MainViewModel.HeaderViewModel> Headers { get; } = new();

    public int FieldWidth => (_numberOfCellsHorizontally + 1) * CellViewModel.CellSize;
    public int FieldHeight => (_numberOfCellsVertically + 1) * CellViewModel.CellSize;

    public GridManagerService()
    {
        InitializeGrid(_numberOfCellsHorizontally, _numberOfCellsVertically);
    }

    public void InitializeGrid(int horizontalSize, int verticalSize)
    {
        _numberOfCellsHorizontally = horizontalSize;
        _numberOfCellsVertically = verticalSize;

        Cells.Clear();
        Headers.Clear();
        _listAllCellStruct.Clear();

        for (var y = 1; y <= _numberOfCellsVertically; y++)
        {
            for (var x = 1; x <= _numberOfCellsHorizontally; x++)
            {
                var cellVm = new CellViewModel { X = x, Y = y };
                Cells.Add(cellVm);
                _listAllCellStruct.Add(new Cell(x, y));
            }
        }

        for (var y = 0; y <= _numberOfCellsVertically; y++)
        {
            Headers.Add(new MainViewModel.HeaderViewModel { Content = y == 0 ? "" : y.ToString(), X = 0, Y = y });
        }

        for (var x = 1; x <= _numberOfCellsHorizontally; x++)
        {
            Headers.Add(new MainViewModel.HeaderViewModel { Content = x.ToString(), X = x, Y = 0 });
        }

        OnPropertyChanged(nameof(FieldWidth));
        OnPropertyChanged(nameof(FieldHeight));
    }

    public void HandleCellInteraction(CellViewModel cellVm)
    {
        var newColor = (Mouse.LeftButton, Mouse.RightButton) switch
        {
            (MouseButtonState.Pressed, _) => Brushes.Transparent,
            (_, MouseButtonState.Pressed) => Brushes.Black,
            _ => cellVm.Background
        };
        if (newColor == cellVm.Background) return;

        if (_isVerticallyMirror) ColoringHorizontal(cellVm, newColor);
        else if (_isHorizontallyMirror) ColoringVertical(cellVm, newColor);
        else if (_isAllMirror) ColoringAll(cellVm, newColor);
        else if (_isVerticallyMirrorRevers) ColoringHorizontalRevers(cellVm, newColor);
        else if (_isHorizontallyMirrorRevers) ColoringVerticalRevers(cellVm, newColor);
        else cellVm.Background = newColor;
    }

    public void SetMirrorModes(bool isVertically, bool isHorizontally, bool isAll, bool isVerticallyRevers, bool isHorizontallyRevers)
    {
        _isVerticallyMirror = isVertically;
        _isHorizontallyMirror = isHorizontally;
        _isAllMirror = isAll;
        _isVerticallyMirrorRevers = isVerticallyRevers;
        _isHorizontallyMirrorRevers = isHorizontallyRevers;
    }

    public List<Cell> GetEmptyCells()
    {
        var listEmptyCellStruct = new List<Cell>();
        foreach (var cellVm in Cells.Where(c => c.Background == Brushes.Transparent))
        {
            var cellModel = _listAllCellStruct.First(c => c.X == cellVm.X && c.Y == cellVm.Y);
            listEmptyCellStruct.Add(cellModel);
        }

        return listEmptyCellStruct;
    }

    public CellViewModel? FindCellVm(int x, int y) => Cells.FirstOrDefault(c => c.X == x && c.Y == y);

    public void ClearAllCellsContent()
    {
        foreach (var cellVm in Cells)
        {
            cellVm.Content = null;
        }
    }

    public List<Cell> LoadGridFromStruct(string[] listEmptyCellStruct)
    {
        foreach (var cellVm in Cells)
        {
            cellVm.Background = Brushes.Black;
            cellVm.Content = null;
        }

        var emptyCells = new List<Cell>();

        foreach (var item in listEmptyCellStruct)
        {
            var strings = item.Split(';');
            if (strings.Length == 2 && int.TryParse(strings[0], out var x) && int.TryParse(strings[1], out var y))
            {
                var cellVm = FindCellVm(x, y);
                if (cellVm != null)
                {
                    cellVm.Background = Brushes.Transparent;
                }

                var cellModel = _listAllCellStruct.FirstOrDefault(c => c.X == x && c.Y == y);
                if (cellModel != null)
                {
                    emptyCells.Add(cellModel);
                }
            }
        }

        return emptyCells;
    }

    #region Coloring Logic

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

    #endregion
}