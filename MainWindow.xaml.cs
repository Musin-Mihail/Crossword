using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Threading.Tasks;
namespace Crossword
{
    public partial class MainWindow : Window
    {
        List<List<string>> listWordsList = new List<List<string>>();
        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();
        PlayingField playingField = new PlayingField();
        SaveLoad saveLoad = new SaveLoad();
        GridFill gridFill = new GridFill();
        Screenshot screenshot = new Screenshot();
        public MainWindow()
        {
            InitializeComponent();
            CreatingThePlayingField();
        }
        void CreatingThePlayingField()
        {
            AddingWatermarks();
            listAllCellStruct = playingField.CreateUIGrid(TheGrid, MoveChangeColor, ClickChangeColor);
            listWordsList = playingField.CreateDictionary();
        }
        async void GridFill()
        {
            Reset.Visibility = Visibility.Hidden;
            GenButton.Visibility = Visibility.Hidden;
            GenStopButton.Visibility = Visibility.Visible;
            await Task.Delay(100);
            listEmptyCellStruct = playingField.SearchForEmptyCells();
            if (listEmptyCellStruct.Count > 0)
            {
                gridFill.AddListAllEmptyWordsLabelVisual(listAllCellStruct, listEmptyCellStruct, listWordsList, WindowsText, Visualization);
                int maxCounGen = 0;
                int maxCounWord = 0;
                try
                {
                    maxCounGen = Int32.Parse(CountGen.Text);
                    maxCounWord = Int32.Parse(CountGenWord.Text);
                }
                catch
                {
                    MessageBox.Show("ОШИБКА. Водите только цифры");
                }
                await gridFill.Generation(maxCounGen, maxCounWord);
            }
            Reset.Visibility = Visibility.Visible;
            GenStopButton.Visibility = Visibility.Hidden;
            GenButton.Visibility = Visibility.Visible;
        }
        void AddingWatermarks()
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
            listEmptyCellStruct = playingField.SearchForEmptyCells();
            saveLoad.Save(listEmptyCellStruct);
        }
        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            saveLoad.Load(listAllCellStruct);
        }
        private void MoveChangeColor(object sender, MouseEventArgs e)
        {
            ChangeColorBlackWhite(sender);
        }
        private void ClickChangeColor(object sender, MouseButtonEventArgs e)
        {
            ChangeColorBlackWhite(sender);
        }
        void ChangeColorBlackWhite(object sender)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Border myBorder = (Border)sender;
                myBorder.Background = Brushes.Transparent;
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Border myBorder = (Border)sender;
                myBorder.Background = Brushes.Black;
            }
        }
        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {
            GridFill();
        }
        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            gridFill.STOP = true;
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
                cell.border.Background = Brushes.Black;
            }
        }

        private void Button_Screenshot(object sender, RoutedEventArgs e)
        {
            screenshot.CreateImage(listAllCellStruct, gridFill.listWordStruct);
        }
    }
}