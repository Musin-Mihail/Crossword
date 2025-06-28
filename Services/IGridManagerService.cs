using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Crossword.Objects;
using Crossword.ViewModel;

namespace Crossword.Services;

public interface IGridManagerService : INotifyPropertyChanged
{
    ObservableCollection<CellViewModel> Cells { get; }
    ObservableCollection<MainViewModel.HeaderViewModel> Headers { get; }
    int FieldWidth { get; }
    int FieldHeight { get; }
    void InitializeGrid(int horizontalSize, int verticalSize);
    void HandleCellInteraction(CellViewModel cellVm);
    void SetMirrorModes(bool isVertically, bool isHorizontally, bool isAll, bool isVerticallyRevers, bool isHorizontallyRevers);
    List<Cell> GetEmptyCells();
    CellViewModel? FindCellVm(int x, int y);
    void ClearAllCellsContent();
    List<Cell> LoadGridFromStruct(string[] listEmptyCellStruct);
}