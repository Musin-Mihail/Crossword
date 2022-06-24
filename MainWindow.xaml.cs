using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
namespace Crossword
{
    // Установка цифр в начале слов. скриншоты заполненной и пустой сетки с определениями
    // Создать структуру которая будет ставить слово, потом перебирать все слова, если не подходит, менять предыдущее и пробовать снова
    public partial class MainWindow : Window
    {
        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();
        SaveLoad saveLoad = new SaveLoad();
        GridGeneration gridGeneration = new GridGeneration();
        UiElements uiElements = new UiElements();
        public MainWindow()
        {
            InitializeComponent();
            gridGeneration.CreateDictionary();
            uiElements.AddElements(GenButton, GenStopButton, VisualCheckBox, WindowsText, CountGoodGen, CountGen);
            CreateUIGrid();
        }
        void CreateUIGrid()
        {
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 30; x++)
                {
                    Label myLabel = CreateLabel();
                    listLabel.Add(myLabel);
                    TheGrid.Children.Add(myLabel);
                    Grid.SetColumn(myLabel, x);
                    Grid.SetRow(myLabel, y);

                    Border myBorder = CreateBorder();
                    listBorder.Add(myBorder);
                    TheGrid.Children.Add(myBorder);
                    Grid.SetColumn(myBorder, x);
                    Grid.SetRow(myBorder, y);
                }
            }
            saveLoad.AddAllBorder(listBorder);
            saveLoad.AddAllLabel(listLabel);
        }
        Border CreateBorder()
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.MouseEnter += new MouseEventHandler(MoveChangeColor);
            myBorder.MouseDown += new MouseButtonEventHandler(ClickChangeColor);
            return myBorder;
        }
        Label CreateLabel()
        {
            Label myLabel = new Label();
            myLabel.HorizontalAlignment = HorizontalAlignment.Center;
            myLabel.VerticalAlignment = VerticalAlignment.Center;
            return myLabel;
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
            gridGeneration.Generation(uiElements, listLabel, listBorder);
        }
        private void Button_ClickSaveGrid(object sender, RoutedEventArgs e)
        {
            saveLoad.Save();
            SaveGrid.Content = "Сетка сохранена";
        }
        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            saveLoad.Load();
            SaveGrid.Content = "Сетка загружена";
        }
        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            gridGeneration.STOP = true;
        }
    }
}