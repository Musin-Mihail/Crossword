using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Crossword.Services;

namespace Crossword.ViewModel;

public class GridControlViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IGridManagerService _gridManagerService;
    private int _numberOfCellsHorizontally = 30;
    private int _numberOfCellsVertically = 30;
    private bool _isVerticallyMirror;
    private bool _isHorizontallyMirror;
    private bool _isAllMirror;
    private bool _isVerticallyMirrorRevers;
    private bool _isHorizontallyMirrorRevers;
    public ICommand ChangeFieldSizeCommand { get; }
    public ICommand CellInteractionCommand { get; }

    public GridControlViewModel(IDialogService dialogService, IGridManagerService gridManagerService, Func<bool> canInteract)
    {
        _dialogService = dialogService;
        _gridManagerService = gridManagerService;

        ChangeFieldSizeCommand = new RelayCommand(_ => ChangeFieldSize());
        CellInteractionCommand = new RelayCommand(HandleCellInteraction, _ => canInteract());

        _gridManagerService.PropertyChanged += OnGridManagerPropertyChanged;
        UpdateMirrorLines();
    }

    private void OnGridManagerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IGridManagerService.FieldWidth)) OnPropertyChanged(nameof(FieldWidth));
        if (e.PropertyName == nameof(IGridManagerService.FieldHeight)) OnPropertyChanged(nameof(FieldHeight));
    }

    public int FieldWidth => _gridManagerService.FieldWidth;
    public int FieldHeight => _gridManagerService.FieldHeight;

    #region Mirror Properties

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
        UpdateMirrorLines();
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
}