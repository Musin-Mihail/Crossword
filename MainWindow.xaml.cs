using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace Crossword
{
    //Удалить жёлтые отметки. Установка цифр в начале слов. скриншоты заполненной и пустой сетки.
    public partial class MainWindow : Window
    {
        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();
        bool number = false;
        string Data = File.ReadAllText("dict.txt");
        List<string> words;
        ClassMove classMove = new ClassMove();
        List<Border> yellowListborder = new List<Border>();
        List<Border> saveYellowListborder = new List<Border>();
        List<Border> saveWhiteListborder = new List<Border>();


        public MainWindow()
        {
            InitializeComponent();
            words = new List<string>(Data.Split('\n'));
            CreateGrid();
        }
        void CreateGrid()
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
            if (Mouse.LeftButton == MouseButtonState.Pressed && number == false)
            {
                ChangeColorBlackWhite(sender);
            }
        }
        private void ClickChangeColor(object sender, MouseButtonEventArgs e)
        {
            if (number == false)
            {
                ChangeColorBlackWhite(sender);
            }
            else
            {
                ChangeColorWhiteYellow(sender);
            }
        }
        void ChangeColorBlackWhite(object sender)
        {
            Border myBorder = (Border)sender;
            if (myBorder.Background == Brushes.Transparent)
            {
                myBorder.Background = Brushes.Black;
            }
            else
            {
                myBorder.Background = Brushes.Transparent;
            }
        }
        void ChangeColorWhiteYellow(object sender)
        {
            Border myBorder = (Border)sender;
            if (myBorder.Background == Brushes.Transparent)
            {
                myBorder.Background = Brushes.Yellow;
            }
            else if (myBorder.Background == Brushes.Yellow)
            {
                myBorder.Background = Brushes.Transparent;
            }
        }
        private void Button_ClickNumber(object sender, RoutedEventArgs e)
        {
            number = true;
            Testing.Text = "Назначай начало слов";
        }
        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {

            bool check = false;
            yellowListborder.Clear();
            foreach (var border in listBorder)
            {
                if (border.Background == Brushes.Yellow)
                {
                    yellowListborder.Add(border);
                }
            }
            foreach (var label in listLabel)
            {
                label.Content = null;
            }
            int count = 0;
            int countGen = 0;
            try
            {
                countGen = Int32.Parse(CountGen.Text);
            }
            catch
            {
                MessageBox.Show("Вводите только цифры в количество генераций");
            }
            while (count < countGen && yellowListborder.Count > 0)
            {
                Worlds.Content = "";
                count++;
                foreach (var border in yellowListborder)
                {
                    if (border.Background == Brushes.Yellow)
                    {
                        classMove.MoveRight(border, listBorder, listLabel, words, Worlds);
                    }
                }
                foreach (var border in yellowListborder)
                {
                    if (border.Background == Brushes.Yellow)
                    {
                        check = classMove.MoveDown(border, listBorder, listLabel, words, Worlds);
                        if (check == false)
                        {
                            break;
                        }
                    }
                }
                if (check == false)
                {
                    foreach (var border in yellowListborder)
                    {
                        border.Background = Brushes.Yellow;
                    }
                    foreach (var label in listLabel)
                    {
                        label.Content = null;
                    }
                }
                else
                {
                    foreach (var border in yellowListborder)
                    {
                        border.Background = Brushes.Transparent;
                    }
                    yellowListborder.Clear();
                }
            }
            if (check == false)
            {
                MessageBox.Show("Генерация не удалась");
            }
            else
            {
                MessageBox.Show("Генерация прошла успешно");

            }
            //foreach (var border in yellowListborder)
            //{
            //    if (check == true)
            //    {
            //        border.Background = Brushes.Transparent;
            //    }
            //    else
            //    {
            //        border.Background = Brushes.Yellow;
            //    }
            //}
        }
        private void Button_ClickGrid(object sender, RoutedEventArgs e)
        {
            number = false;
            Testing.Text = "Рисуй сетку";
        }
        private void Button_ClickSaveGrid(object sender, RoutedEventArgs e)
        {
            foreach (var border in listBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    saveWhiteListborder.Add(border);
                }
                else if (border.Background == Brushes.Yellow)
                {
                    saveYellowListborder.Add(border);
                }
            }
            SaveGrid.Content = "Сетка сохранена";
        }
        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            foreach (var label in listLabel)
            {
                label.Content = "";
            }
            foreach (var border in listBorder)
            {
                border.Background = Brushes.Black;
            }
            foreach (var border in saveWhiteListborder)
            {
                border.Background = Brushes.Transparent;
            }
            foreach (var border in saveYellowListborder)
            {
                border.Background = Brushes.Yellow;
            }
            SaveGrid.Content = "Сетка загружена";
        }
    }
}