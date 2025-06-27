using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Main;
using Crossword.PlayingField;
using Crossword.SaveLoad;
using Crossword.Screenshot;
using Crossword.ViewModel;

namespace Crossword;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private static int _numberOfCellsHorizontally = 30;
    private static int _numberOfCellsVertically = 30;
    private const int CellSize = 30;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
        CreatingThePlayingField();
    }

    private async void Button_ClickGen(object sender, RoutedEventArgs e)
    {
        _viewModel.MaxSecondsText = MaxSeconds.Text;
        _viewModel.TaskDelayText = TaskDelay.Text;
        _viewModel.IsVisualizationChecked = Visualization.IsChecked ?? false;
        await _viewModel.StartGenerationAsync();
    }

    private void Button_ClickStop(object sender, RoutedEventArgs e)
    {
        _viewModel.StopGeneration();
    }

    #region Unchanged_Code

    private void CreatingThePlayingField()
    {
        ResetDict.Get();
        SelectedDictionary.Content = App.GameState.SelectedDictionaryInfo;
        CreateUiGrid.Get(TheGrid, MoveChangeColor, ClickChangeColor, _numberOfCellsHorizontally, _numberOfCellsVertically, CellSize);
        LineCenterH.X1 = _numberOfCellsHorizontally * 30 / 2 + 30;
        LineCenterH.X2 = _numberOfCellsHorizontally * 30 / 2 + 30;
        LineCenterH.Y2 = _numberOfCellsVertically * 30 + 60;
        LineCenterV.Y1 = _numberOfCellsVertically * 30 / 2 + 30;
        LineCenterV.Y2 = _numberOfCellsVertically * 30 / 2 + 30;
        LineCenterV.X2 = _numberOfCellsHorizontally * 30 + 60;
    }

    private void Button_Basic_Dictionary(object sender, RoutedEventArgs e)
    {
        _viewModel.ResetDictionaries();
        MessageBox.Show("Выбран основной словарь");
    }

    private void Button_ClickSaveGrid(object sender, RoutedEventArgs e)
    {
        SearchForEmptyCells.Get();
        Save.Get();
    }

    private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
    {
        var loadGrid = new LoadGrid();
        loadGrid.ShowDialog();
        if (loadGrid.Ready)
        {
            Load.Get(loadGrid.ListEmptyCellStruct);
        }
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

    private static void ColoringVerticalRevers(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in App.GameState.ListAllCellStruct)
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

    private static void ColoringVertical(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in App.GameState.ListAllCellStruct)
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

    private static void ColoringHorizontalRevers(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in App.GameState.ListAllCellStruct)
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

    private static void ColoringHorizontal(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in App.GameState.ListAllCellStruct)
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

    private static void ColoringAll(Border b, Brush c)
    {
        int x = 0, y = 0;
        foreach (var cell in App.GameState.ListAllCellStruct)
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

    private static void ColoringCell(int x, int y, Brush color)
    {
        foreach (var cell in App.GameState.ListAllCellStruct)
            if (cell.X == x && cell.Y == y)
            {
                cell.Border.Background = color;
                break;
            }
    }

    private void Button_Reset(object s, RoutedEventArgs e)
    {
        foreach (var cell in App.GameState.ListAllCellStruct)
        {
            cell.Label.Content = null;
            cell.Border.Background = Brushes.Black;
        }
    }

    private void Button_Screenshot(object s, RoutedEventArgs e)
    {
        if (App.GameState.ListEmptyCellStruct.Count > 1) CreateImage.Get();
        else MessageBox.Show("Ячеек меньше двух\nИли не было генерации");
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

    private void Button_RequiredDictionary(object s, RoutedEventArgs e)
    {
        new RequiredDictionary().ShowDialog();
    }

    private void Button_DictionariesSelection(object sender, RoutedEventArgs e)
    {
        var dictionariesSelection = new DictionariesSelection();
        dictionariesSelection.ShowDialog();
        if (dictionariesSelection.Ready)
        {
            App.GameState.ListDictionaries.Clear();
            var message = "Выбранные словари:\n";
            var dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
            foreach (var selectedDictionaries in dictionariesSelection.SelectedDictionaries)
            {
                var list = new List<string>(selectedDictionaries.Split(';'));
                foreach (var path in dictionariesPaths)
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    if (list[0] == name)
                    {
                        message += selectedDictionaries + "\n";
                        var dictionary = CreateDictionary.Get(path);
                        dictionary.Name = name;
                        dictionary.MaxCount = int.Parse(list[1]);
                        App.GameState.ListDictionaries.Add(dictionary);
                        break;
                    }
                }
            }

            var commonDictionary = CreateDictionary.Get("dict.txt");
            App.GameState.ListDictionaries.Add(commonDictionary);
            App.GameState.ListDictionaries[^1].Name = "Общий";
            App.GameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
            MessageBox.Show(message);
            _viewModel.SelectedDictionaryInfo = message;
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