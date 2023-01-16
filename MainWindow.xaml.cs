using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.FormationOfAQueue;
using Crossword.GridFill;
using Crossword.GridFill.SelectionAndInstallation;
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
            AddingWatermarks();
            CreatingThePlayingField();
        }

        private void CreatingThePlayingField()
        {
            ResetDict();
            Global.windowsText = WindowsTextTop;
            Global.visualization = Visualization;
            Global.gridGeneration = GridGeneration;
            Global.startGeneration = GenButton;
            Global.stopGeneration = GenStopButton;
            Global.difficultyLevel = DifficultyLevel;

            CreateUiGrid.Get(TheGrid, MoveChangeColor, ClickChangeColor, _numberOfCellsHorizontally, _numberOfCellsVertically, CellSize);
            LineCenterH.X1 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.X2 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.Y2 = _numberOfCellsVertically * 30 + 60;
            LineCenterV.Y1 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.Y2 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.X2 = _numberOfCellsHorizontally * 30 + 60;
        }

        private void ResetDict()
        {
            Global.listDictionaries.Clear();

            Dictionary commonDictionary = CreateDictionary.Get("dict.txt");
            Global.listDictionaries.Add(commonDictionary);
            Global.listDictionaries[^1].name = "Общий";
            Global.listDictionaries[^1].maxCount = commonDictionary.words.Count;

            SelectedDictionary.Content = "Основной словарь";
        }

        private async Task GridFill()
        {
            await Task.Delay(100);
            SearchForEmptyCells.Get();
            if (Global.listEmptyCellStruct.Count > 0)
            {
                FormationQueue.Get();
                try
                {
                    Global.maxSeconds = int.Parse(MaxSeconds.Text);
                    Global.taskDelay = int.Parse(TaskDelay.Text);
                }
                catch
                {
                    MessageBox.Show("ОШИБКА. Водите только цифры");
                }

                SelectionAndInstallationOfWords.Get();
            }
        }

        private void AddingWatermarks()
        {
            for (int i = 0; i < 50; i++)
            {
                for (int y = 0; y < 6; y++)
                {
                    MusinMihail.Content += "Разработчик Мусин Михаил. ";
                }

                MusinMihail.Content += "\n";
            }
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
            if (loadGrid.ready == true)
            {
                Load.Get(loadGrid.listEmptyCellStruct);
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
                Border myBorder = (Border)sender;
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
                Border myBorder = (Border)sender;
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

        static void ColoringVerticalRevers(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.border == myBorder)
                {
                    x = cell.x;
                    y = cell.y;
                    break;
                }
            }

            int center = _numberOfCellsHorizontally / 2;
            if (x <= center)
            {
                myBorder.Background = color;
                int mirrorX = _numberOfCellsHorizontally - x + 1;
                int mirrorY = _numberOfCellsVertically - y + 1;
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

        static void ColoringVertical(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.border == myBorder)
                {
                    x = cell.x;
                    y = cell.y;
                    break;
                }
            }

            int center = _numberOfCellsHorizontally / 2;
            if (x <= center)
            {
                myBorder.Background = color;
                int mirrorX = _numberOfCellsHorizontally - x + 1;
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

        static void ColoringHorizontalRevers(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.border == myBorder)
                {
                    x = cell.x;
                    y = cell.y;
                    break;
                }
            }

            int center = _numberOfCellsVertically / 2;
            if (y <= center)
            {
                myBorder.Background = color;
                int mirrorX = _numberOfCellsHorizontally - x + 1;
                int mirrorY = _numberOfCellsVertically - y + 1;
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

        static void ColoringHorizontal(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.border == myBorder)
                {
                    x = cell.x;
                    y = cell.y;
                    break;
                }
            }

            int center = _numberOfCellsVertically / 2;
            if (y <= center)
            {
                myBorder.Background = color;
                int mirrorY = _numberOfCellsVertically - y + 1;
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

        static void ColoringAll(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.border == myBorder)
                {
                    x = cell.x;
                    y = cell.y;
                    break;
                }
            }

            int centerH = _numberOfCellsHorizontally / 2;
            int centerV = _numberOfCellsVertically / 2;
            if (x <= centerH && y <= centerV)
            {
                myBorder.Background = color;
                int mirrorX = _numberOfCellsHorizontally - x + 1;
                int mirrorY = _numberOfCellsVertically - y + 1;
                ColoringCell(mirrorX, y, color);
                ColoringCell(x, mirrorY, color);
                ColoringCell(mirrorX, mirrorY, color);
            }

            if (_numberOfCellsHorizontally % 2 != 0)
            {
                if (x == centerH + 1 && y <= centerV)
                {
                    myBorder.Background = color;
                    int mirrorY = _numberOfCellsVertically - y + 1;
                    ColoringCell(x, mirrorY, color);
                }
            }

            if (_numberOfCellsVertically % 2 != 0)
            {
                if (x <= centerH && y == centerV + 1)
                {
                    myBorder.Background = color;
                    int mirrorX = _numberOfCellsHorizontally - x + 1;
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

        static void ColoringCell(int x, int y, Brush color)
        {
            foreach (Cell cell in Global.listAllCellStruct)
            {
                if (cell.x == x && cell.y == y)
                {
                    cell.border.Background = color;
                    break;
                }
            }
        }

        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {
            GridFill();
        }

        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            StopGeneration.Get();
            Global.stop = true;
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in Global.listAllCellStruct)
            {
                cell.label.Content = null;
                cell.border.Background = Brushes.Black;
            }
        }

        private void Button_Screenshot(object sender, RoutedEventArgs e)
        {
            if (Global.listEmptyCellStruct.Count > 1)
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
            if (сhangeFill.ready == true)
            {
                _numberOfCellsHorizontally = сhangeFill.numberOfCellsHorizontally;
                _numberOfCellsVertically = сhangeFill.numberOfCellsVertically;
                CreatingThePlayingField();
            }
        }

        private void Button_DictionariesSelection(object sender, RoutedEventArgs e)
        {
            var dictionariesSelection = new DictionariesSelection();
            dictionariesSelection.ShowDialog();
            if (dictionariesSelection.ready == true)
            {
                Global.listDictionaries.Clear();
                string message = "Выбранные словари:\n";
                List<string> dictionariesPaths = Directory.GetFiles("Dictionaries/").ToList();
                foreach (var selectedDictionaries in dictionariesSelection.selectedDictionaries)
                {
                    List<string> list = new List<string>(selectedDictionaries.Split(';'));
                    foreach (var path in dictionariesPaths)
                    {
                        string name = Path.GetFileNameWithoutExtension(path);
                        if (list[0] == name)
                        {
                            message += selectedDictionaries + "\n";
                            Dictionary dictionary = CreateDictionary.Get(path);
                            dictionary.name = name;
                            dictionary.maxCount = int.Parse(list[1]);
                            Global.listDictionaries.Add(dictionary);
                            break;
                        }
                    }
                }

                Dictionary commonDictionary = CreateDictionary.Get("dict.txt");

                Global.listDictionaries.Add(commonDictionary);
                Global.listDictionaries[^1].name = "Общий";
                Global.listDictionaries[^1].maxCount = commonDictionary.words.Count;
                MessageBox.Show(message);
                SelectedDictionary.Content = message;
            }
        }

        private void Button_Basic_Dictionary(object sender, RoutedEventArgs e)
        {
            ResetDict();
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