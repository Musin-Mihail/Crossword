using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.GridFill.SelectionAndInstallation;
using Crossword.Main;
using Crossword.Objects;
using Crossword.PlayingField;
using Crossword.SaveLoad;
using Crossword.Screenshot;

namespace Crossword
{
    public partial class MainWindow
    {
        private static int _numberOfCellsHorizontally = 30;
        private static int _numberOfCellsVertically = 30;
        private const int CellSize = 30;

        public MainWindow()
        {
            InitializeComponent();
            CreatingThePlayingField();
        }

        private void CreatingThePlayingField()
        {
            Global.windowsText = WindowsTextTop;
            Global.visualization = Visualization;
            Global.gridGeneration = GridGeneration;
            Global.startGeneration = GenButton;
            Global.stopGeneration = GenStopButton;
            Global.difficultyLevel = DifficultyLevel;
            Global.selectedDictionary = SelectedDictionary;
            ResetDict.Get();
            CreateUiGrid.Get(TheGrid, MoveChangeColor, ClickChangeColor, _numberOfCellsHorizontally,
                _numberOfCellsVertically, CellSize);
            LineCenterH.X1 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.X2 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.Y2 = _numberOfCellsVertically * 30 + 60;
            LineCenterV.Y1 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.Y2 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.X2 = _numberOfCellsHorizontally * 30 + 60;
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
                if (VerticallyMirror.IsChecked == true)
                {
                    ColoringHorizontal(myBorder, Brushes.Transparent);
                }
                else if (HorizontallyMirror.IsChecked == true)
                {
                    ColoringVertical(myBorder, Brushes.Transparent);
                }
                else if (AllMirror.IsChecked == true)
                {
                    ColoringAll(myBorder, Brushes.Transparent);
                }
                else if (VerticallyMirrorRevers.IsChecked == true)
                {
                    ColoringHorizontalRevers(myBorder, Brushes.Transparent);
                }
                else if (HorizontallyMirrorRevers.IsChecked == true)
                {
                    ColoringVerticalRevers(myBorder, Brushes.Transparent);
                }
                else
                {
                    myBorder.Background = Brushes.Transparent;
                }
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                var myBorder = (Border)sender;
                if (VerticallyMirror.IsChecked == true)
                {
                    ColoringHorizontal(myBorder, Brushes.Black);
                }
                else if (HorizontallyMirror.IsChecked == true)
                {
                    ColoringVertical(myBorder, Brushes.Black);
                }
                else if (AllMirror.IsChecked == true)
                {
                    ColoringAll(myBorder, Brushes.Black);
                }
                else if (VerticallyMirrorRevers.IsChecked == true)
                {
                    ColoringHorizontalRevers(myBorder, Brushes.Black);
                }
                else if (HorizontallyMirrorRevers.IsChecked == true)
                {
                    ColoringVerticalRevers(myBorder, Brushes.Black);
                }
                else
                {
                    myBorder.Background = Brushes.Black;
                }
            }
        }

        private static void ColoringVerticalRevers(Border myBorder, Brush color)
        {
            var x = 0;
            var y = 0;
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.Border == myBorder)
                {
                    x = cell.X;
                    y = cell.Y;
                    break;
                }
            }

            var center = _numberOfCellsHorizontally / 2;
            if (x <= center)
            {
                myBorder.Background = color;
                var mirrorX = _numberOfCellsHorizontally - x + 1;
                var mirrorY = _numberOfCellsVertically - y + 1;
                ColoringCell(mirrorX, mirrorY, color);
            }

            if (_numberOfCellsHorizontally % 2 != 0)
            {
                if (x == center + 1)
                {
                    myBorder.Background = color;
                }
            }
        }

        private static void ColoringVertical(Border myBorder, Brush color)
        {
            var x = 0;
            var y = 0;
            foreach (Cell cell in Global.ListAllCellStruct)
            {
                if (cell.Border == myBorder)
                {
                    x = cell.X;
                    y = cell.Y;
                    break;
                }
            }

            var center = _numberOfCellsHorizontally / 2;
            if (x <= center)
            {
                myBorder.Background = color;
                var mirrorX = _numberOfCellsHorizontally - x + 1;
                ColoringCell(mirrorX, y, color);
            }

            if (_numberOfCellsHorizontally % 2 != 0)
            {
                if (x == center + 1)
                {
                    myBorder.Background = color;
                }
            }
        }

        private static void ColoringHorizontalRevers(Border myBorder, Brush color)
        {
            var x = 0;
            var y = 0;
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.Border == myBorder)
                {
                    x = cell.X;
                    y = cell.Y;
                    break;
                }
            }

            var center = _numberOfCellsVertically / 2;
            var mirrorY = _numberOfCellsVertically - y + 1;
            if (y <= center)
            {
                myBorder.Background = color;
                var mirrorX = _numberOfCellsHorizontally - x + 1;
                ColoringCell(mirrorX, mirrorY, color);
            }

            if (_numberOfCellsVertically % 2 != 0)
            {
                if (y == center + 1)
                {
                    myBorder.Background = color;
                }
            }
        }

        private static void ColoringHorizontal(Border myBorder, Brush color)
        {
            var x = 0;
            var y = 0;
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.Border == myBorder)
                {
                    x = cell.X;
                    y = cell.Y;
                    break;
                }
            }

            var center = _numberOfCellsVertically / 2;
            if (y <= center)
            {
                myBorder.Background = color;
                var mirrorY = _numberOfCellsVertically - y + 1;
                ColoringCell(x, mirrorY, color);
            }

            if (_numberOfCellsVertically % 2 != 0)
            {
                if (y == center + 1)
                {
                    myBorder.Background = color;
                }
            }
        }

        private static void ColoringAll(Border myBorder, Brush color)
        {
            var x = 0;
            var y = 0;
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.Border == myBorder)
                {
                    x = cell.X;
                    y = cell.Y;
                    break;
                }
            }

            var centerH = _numberOfCellsHorizontally / 2;
            var centerV = _numberOfCellsVertically / 2;
            if (x <= centerH && y <= centerV)
            {
                myBorder.Background = color;
                var mirrorX = _numberOfCellsHorizontally - x + 1;
                var mirrorY = _numberOfCellsVertically - y + 1;
                ColoringCell(mirrorX, y, color);
                ColoringCell(x, mirrorY, color);
                ColoringCell(mirrorX, mirrorY, color);
            }

            if (_numberOfCellsHorizontally % 2 != 0)
            {
                if (x == centerH + 1 && y <= centerV)
                {
                    myBorder.Background = color;
                    var mirrorY = _numberOfCellsVertically - y + 1;
                    ColoringCell(x, mirrorY, color);
                }
            }

            if (_numberOfCellsVertically % 2 != 0)
            {
                int mirrorX = _numberOfCellsHorizontally - x + 1;
                if (x <= centerH && y == centerV + 1)
                {
                    myBorder.Background = color;
                    ColoringCell(mirrorX, y, color);
                }
            }

            if (_numberOfCellsHorizontally % 2 != 0 && _numberOfCellsVertically % 2 != 0)
            {
                if (x == centerH + 1 && y == centerV + 1)
                {
                    myBorder.Background = color;
                }
            }
        }

        private static void ColoringCell(int x, int y, Brush color)
        {
            foreach (var cell in Global.ListAllCellStruct)
            {
                if (cell.X == x && cell.Y == y)
                {
                    cell.Border.Background = color;
                    break;
                }
            }
        }

        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {
            GridFillMain.Get(MaxSeconds.Text, TaskDelay.Text);
        }

        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            StopGeneration.Get();
            Global.stop = true;
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            foreach (var cell in Global.ListAllCellStruct)
            {
                cell.Label.Content = null;
                cell.Border.Background = Brushes.Black;
            }
        }

        private void Button_Screenshot(object sender, RoutedEventArgs e)
        {
            if (Global.ListEmptyCellStruct.Count > 1)
            {
                CreateImage.Get();
            }
            else
            {
                MessageBox.Show("Ячеек меньше двух\nИли не было генерации");
            }
        }

        private void Button_ChangeFill(object sender, RoutedEventArgs e)
        {
            var сhangeFill = new сhangeFill();
            сhangeFill.ShowDialog();
            if (сhangeFill.Ready)
            {
                _numberOfCellsHorizontally = сhangeFill.NumberOfCellsHorizontally;
                _numberOfCellsVertically = сhangeFill.NumberOfCellsVertically;
                CreatingThePlayingField();
            }
        }

        private void Button_RequiredDictionary(object sender, RoutedEventArgs e)
        {
            var requiredDictionary = new RequiredDictionary();
            requiredDictionary.ShowDialog();
        }

        private void Button_DictionariesSelection(object sender, RoutedEventArgs e)
        {
            var dictionariesSelection = new DictionariesSelection();
            dictionariesSelection.ShowDialog();
            if (dictionariesSelection.Ready)
            {
                Global.ListDictionaries.Clear();
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
                            Global.ListDictionaries.Add(dictionary);
                            break;
                        }
                    }
                }

                var commonDictionary = CreateDictionary.Get("dict.txt");
                Global.ListDictionaries.Add(commonDictionary);
                Global.ListDictionaries[^1].Name = "Общий";
                Global.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
                MessageBox.Show(message);
                SelectedDictionary.Content = message;
            }
        }

        private void Button_Basic_Dictionary(object sender, RoutedEventArgs e)
        {
            ResetDict.Get();
            MessageBox.Show("Выбран основной словарь");
        }

        private void ClearMirror_OnChecked(object sender, RoutedEventArgs e)
        {
            if (VerticallyMirror.IsChecked == true)
            {
                LineCenterH.Visibility = Visibility.Hidden;
                LineCenterV.Visibility = Visibility.Visible;
            }
            else if (HorizontallyMirror.IsChecked == true)
            {
                LineCenterH.Visibility = Visibility.Visible;
                LineCenterV.Visibility = Visibility.Hidden;
            }
            else if (AllMirror.IsChecked == true)
            {
                LineCenterH.Visibility = Visibility.Visible;
                LineCenterV.Visibility = Visibility.Visible;
            }
            else if (VerticallyMirrorRevers.IsChecked == true)
            {
                LineCenterH.Visibility = Visibility.Hidden;
                LineCenterV.Visibility = Visibility.Visible;
            }
            else if (HorizontallyMirrorRevers.IsChecked == true)
            {
                LineCenterH.Visibility = Visibility.Visible;
                LineCenterV.Visibility = Visibility.Hidden;
            }
            else
            {
                LineCenterH.Visibility = Visibility.Hidden;
                LineCenterV.Visibility = Visibility.Hidden;
            }
        }
    }
}