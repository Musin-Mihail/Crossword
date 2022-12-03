using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crossword
{
    public partial class MainWindow
    {
        private List<List<string>> _listWordsList = new();
        private static List<Cell> _listAllCellStruct = new();
        private List<Cell> _listEmptyCellStruct = new();
        private readonly PlayingField _playingField = new();
        private readonly SaveLoad _saveLoad = new();
        private readonly GridFill _gridFill = new();
        private readonly Screenshot _screenshot = new();
        private readonly FormationOfAQueue _formationOfAQueue = new();
        private List<Word> _listWordStruct = new();
        private static int _numberOfCellsHorizontally = 30;
        private static int _numberOfCellsVertically = 30;

        public bool horizontallyMirror = false;
        public bool verticallyMirror = false;
        public bool allMirror = false;

        public MainWindow()
        {
            InitializeComponent();
            AddingWatermarks();
            CreatingThePlayingField();
        }

        private void CreatingThePlayingField()
        {
            _listAllCellStruct.Clear();
            _listAllCellStruct = _playingField.CreateUiGrid(TheGrid, MoveChangeColor, ClickChangeColor, _numberOfCellsHorizontally, _numberOfCellsVertically);
            _listWordsList = _playingField.CreateDictionary();
            
            LineCenterH.X1 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.X2 = _numberOfCellsHorizontally * 30 / 2 + 30;
            LineCenterH.Y2 = _numberOfCellsVertically * 30 + 60;
            
            LineCenterV.Y1 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.Y2 = _numberOfCellsVertically * 30 / 2 + 30;
            LineCenterV.X2 = _numberOfCellsHorizontally * 30 + 60;
        }

        private async Task GridFill()
        {
            StartGen();
            await Task.Delay(100);
            _listEmptyCellStruct = _playingField.SearchForEmptyCells();
            if (_listEmptyCellStruct.Count > 0)
            {
                _listWordStruct = _formationOfAQueue.FormationQueue(_listEmptyCellStruct);
                int maxCountGen = 0;
                int maxCountWord = 0;
                try
                {
                    maxCountGen = int.Parse(CountGen.Text);
                    maxCountWord = int.Parse(CountGenWord.Text);
                }
                catch
                {
                    MessageBox.Show("ОШИБКА. Водите только цифры");
                }

                await _gridFill.Generation(maxCountGen, maxCountWord, _listWordStruct, _listEmptyCellStruct, _listWordsList, WindowsText, Visualization);
            }
            
            EndGen();
        }
        
        private void StartGen()
        {
            Reset.Visibility = Visibility.Hidden;
            GenButton.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Hidden;
            Load.Visibility = Visibility.Hidden;
            Screenshot.Visibility = Visibility.Hidden;
            GenStopButton.Visibility = Visibility.Visible;
            RadioButtons.Visibility = Visibility.Hidden;
        }

        private void EndGen()
        {
            Reset.Visibility = Visibility.Visible;
            GenButton.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            Load.Visibility = Visibility.Visible;
            Screenshot.Visibility = Visibility.Visible;
            GenStopButton.Visibility = Visibility.Hidden;
            RadioButtons.Visibility = Visibility.Visible;
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
            _listEmptyCellStruct = _playingField.SearchForEmptyCells();
            _saveLoad.Save(_listEmptyCellStruct);
        }

        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            var loadGrid = new LoadGrid();
            loadGrid.ShowDialog();
            if (loadGrid.ready == true)
            {
                _saveLoad.Load(_listAllCellStruct, loadGrid.listEmptyCellStruct);
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
                else
                {
                    myBorder.Background = Brushes.Black;
                }
            }
        }

        static void ColoringVertical(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (var cell in _listAllCellStruct)
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
                if (x == center+1)
                {
                    myBorder.Background = color;
                }
            }
        }
        
        static void ColoringHorizontal(Border myBorder, Brush color)
        {
            int x = 0;
            int y = 0;
            foreach (var cell in _listAllCellStruct)
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
            foreach (var cell in _listAllCellStruct)
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
            foreach (var cell in _listAllCellStruct)
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
            _gridFill.STOP = true;
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in _listAllCellStruct)
            {
                cell.label.Content = null;
                cell.border.Background = Brushes.Black;
            }
        }

        private void Button_Screenshot(object sender, RoutedEventArgs e)
        {
            _screenshot.CreateImage(_listAllCellStruct, _listWordStruct);
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
    }
}