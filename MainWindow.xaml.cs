using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Main;
using Crossword.PlayingField;
using Crossword.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword;

public partial class MainWindow : Window
{
    private static int _numberOfCellsHorizontally = 30;
    private static int _numberOfCellsVertically = 30;
    private const int CellSize = 30;
    private readonly CrosswordState _gameState;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        _gameState = App.ServiceProvider.GetRequiredService<CrosswordState>();

        CreatingThePlayingField();
    }

    #region Unchanged_Code (UI-specific logic remains here)

    private void CreatingThePlayingField()
    {
        var resetDict = new ResetDict(_gameState);
        resetDict.Get();
        CreateUiGrid.Get(TheGrid, MoveChangeColor, ClickChangeColor, _numberOfCellsHorizontally, _numberOfCellsVertically, CellSize, _gameState);
        LineCenterH.X1 = _numberOfCellsHorizontally * 30 / 2 + 30;
        LineCenterH.X2 = _numberOfCellsHorizontally * 30 / 2 + 30;
        LineCenterH.Y2 = _numberOfCellsVertically * 30 + 60;
        LineCenterV.Y1 = _numberOfCellsVertically * 30 / 2 + 30;
        LineCenterV.Y2 = _numberOfCellsVertically * 30 / 2 + 30;
        LineCenterV.X2 = _numberOfCellsHorizontally * 30 + 60;
    }

    private void MoveChangeColor(object sender, MouseEventArgs e)
    {
        ChangeColorBlackWhite(sender);
    }

    private void ClickChangeColor(object sender, MouseButtonEventArgs e)
    {
        ChangeColorBlackWhite(sender);
    }

    private void ChangeColorBlackWhite(object sender)
    {
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            var myBorder = (Border)sender;
            if (VerticallyMirror.IsChecked == true) ColoringHorizontal(myBorder, Brushes.Transparent);
            else if (HorizontallyMirror.IsChecked == true) ColoringVertical(myBorder, Brushes.Transparent);
            else if (AllMirror.IsChecked == true) ColoringAll(myBorder, Brushes.Transparent);
            else if (VerticallyMirrorRevers.IsChecked == true) ColoringHorizontalRevers(myBorder, Brushes.Transparent);
            else if (HorizontallyMirrorRevers.IsChecked == true) ColoringVerticalRevers(myBorder, Brushes.Transparent);
            else myBorder.Background = Brushes.Transparent;
        }
        else if (Mouse.RightButton == MouseButtonState.Pressed)
        {
            var myBorder = (Border)sender;
            if (VerticallyMirror.IsChecked == true) ColoringHorizontal(myBorder, Brushes.Black);
            else if (HorizontallyMirror.IsChecked == true) ColoringVertical(myBorder, Brushes.Black);
            else if (AllMirror.IsChecked == true) ColoringAll(myBorder, Brushes.Black);
            else if (VerticallyMirrorRevers.IsChecked == true) ColoringHorizontalRevers(myBorder, Brushes.Black);
            else if (HorizontallyMirrorRevers.IsChecked == true) ColoringVerticalRevers(myBorder, Brushes.Black);
            else myBorder.Background = Brushes.Black;
        }
    }

    private void ColoringVerticalRevers(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.Border == b)
            {
                x = cell.X;
                y = cell.Y;
                break;
            }

        var center = _numberOfCellsHorizontally / 2;
        if (x <= center)
        {
            b.Background = c;
            var mX = _numberOfCellsHorizontally - x + 1;
            var mY = _numberOfCellsVertically - y + 1;
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && x == center + 1) b.Background = c;
    }

    private void ColoringVertical(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.Border == b)
            {
                x = cell.X;
                y = cell.Y;
                break;
            }

        var center = _numberOfCellsHorizontally / 2;
        if (x <= center)
        {
            b.Background = c;
            var mX = _numberOfCellsHorizontally - x + 1;
            ColoringCell(mX, y, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && x == center + 1) b.Background = c;
    }

    private void ColoringHorizontalRevers(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.Border == b)
            {
                x = cell.X;
                y = cell.Y;
                break;
            }

        var center = _numberOfCellsVertically / 2;
        var mY = _numberOfCellsVertically - y + 1;
        if (y <= center)
        {
            b.Background = c;
            var mX = _numberOfCellsHorizontally - x + 1;
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0 && y == center + 1) b.Background = c;
    }

    private void ColoringHorizontal(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.Border == b)
            {
                x = cell.X;
                y = cell.Y;
                break;
            }

        var center = _numberOfCellsVertically / 2;
        if (y <= center)
        {
            b.Background = c;
            var mY = _numberOfCellsVertically - y + 1;
            ColoringCell(x, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0 && y == center + 1) b.Background = c;
    }

    private void ColoringAll(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.Border == b)
            {
                x = cell.X;
                y = cell.Y;
                break;
            }

        var cH = _numberOfCellsHorizontally / 2;
        var cV = _numberOfCellsVertically / 2;
        if (x <= cH && y <= cV)
        {
            b.Background = c;
            var mX = _numberOfCellsHorizontally - x + 1;
            var mY = _numberOfCellsVertically - y + 1;
            ColoringCell(mX, y, c);
            ColoringCell(x, mY, c);
            ColoringCell(mX, mY, c);
        }

        if (_numberOfCellsHorizontally % 2 != 0 && x == cH + 1 && y <= cV)
        {
            b.Background = c;
            var mY = _numberOfCellsVertically - y + 1;
            ColoringCell(x, mY, c);
        }

        if (_numberOfCellsVertically % 2 != 0)
        {
            var mX = _numberOfCellsHorizontally - x + 1;
            if (x <= cH && y == cV + 1)
            {
                b.Background = c;
                ColoringCell(mX, y, c);
            }
        }

        if (_numberOfCellsHorizontally % 2 != 0 && _numberOfCellsVertically % 2 != 0 && x == cH + 1 && y == cV + 1) b.Background = c;
    }

    private void ColoringCell(int x, int y, Brush color)
    {
        foreach (var cell in _gameState.ListAllCellStruct)
            if (cell.X == x && cell.Y == y)
            {
                cell.Border.Background = color;
                break;
            }
    }

    private void Button_ChangeFill(object s, RoutedEventArgs e)
    {
        var w = new сhangeFill();
        w.ShowDialog();
        if (w.Ready)
        {
            _numberOfCellsHorizontally = w.NumberOfCellsHorizontally;
            _numberOfCellsVertically = w.NumberOfCellsVertically;
            CreatingThePlayingField();
        }
    }

    private void ClearMirror_OnChecked(object s, RoutedEventArgs e)
    {
        LineCenterH.Visibility = Visibility.Hidden;
        LineCenterV.Visibility = Visibility.Hidden;
        if (VerticallyMirror.IsChecked == true || VerticallyMirrorRevers.IsChecked == true) LineCenterV.Visibility = Visibility.Visible;
        if (HorizontallyMirror.IsChecked == true || HorizontallyMirrorRevers.IsChecked == true) LineCenterH.Visibility = Visibility.Visible;
        if (AllMirror.IsChecked == true)
        {
            LineCenterH.Visibility = Visibility.Visible;
            LineCenterV.Visibility = Visibility.Visible;
        }
    }

    #endregion
}