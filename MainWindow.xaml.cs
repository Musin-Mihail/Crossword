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
    // Установка цифр в начале слов. скриншоты заполненной и пустой сетки с определениями
    // Создать структуру которая будет ставить слово, потом перебирать все слова, если не подходит, менять предыдущее и пробовать снова
    public partial class MainWindow : Window
    {
        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();
        List<Border> listWhiteBorder = new List<Border>();
        List<string> words;
        ClassMove classMove = new ClassMove();
        List<Border> saveYellowListborder = new List<Border>();
        List<Border> saveWhiteListborder = new List<Border>();
        public MainWindow()
        {
            InitializeComponent();
            string[] array = File.ReadAllLines("dict.txt");
            words = array.ToList();
            CreateGrid();
        }
        void TestingConvert()
        {
            var ttt = File.ReadAllLines("4word.txt");
            List<string> TestWords = ttt.ToList();
            string AllText = "";
            List<string> temp1;
            List<string> temp2;
            string ShowError = "";
            foreach (var item in TestWords)
            {
                try
                {
                    temp1 = new List<string>(item.Split('-'));
                    if (temp1.Count > 2)
                    {
                        ShowError += "Тире - " + item + "\n";
                    }
                    temp2 = new List<string>(temp1[1].Split('.'));
                    AllText += temp1[0] + ";";
                    for (int i = 0; i < temp2.Count; i++)
                    {
                        if (temp2[i].Length < 3)
                        {
                            ShowError += "Меньше 3 - " + item + "\n";
                        }
                        AllText += temp2[i];
                        if (i != temp2.Count - 1)
                        {
                            AllText += ";";
                        }
                    }
                    AllText += "\n";
                }
                catch
                {
                    ShowError += "Ошибка - " + item + "\n";
                }
            }
            MessageBox.Show(ShowError);
            File.WriteAllText("newText.txt", AllText);
        }
        void TestingConvert2()
        {
            var ttt = File.ReadAllLines("4word.txt");
            List<string> TestWords = ttt.ToList();
            //string AllText = "";
            List<string> temp1;
            string ShowError = "";
            foreach (var item in TestWords)
            {
                temp1 = new List<string>(item.Split(';'));
                ShowError += temp1[0] + " - ";
            }
            MessageBox.Show(ShowError);
            //File.WriteAllText("newText.txt", AllText);
        }
        //void SaveJPG()
        //{
        //    Visual target;
        //    string fileName;

        //    Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

        //    RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

        //    DrawingVisual visual = new DrawingVisual();

        //    using (DrawingContext context = visual.RenderOpen())
        //    {
        //        VisualBrush visualBrush = new VisualBrush(target);
        //        context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
        //    }

        //    renderTarget.Render(visual);
        //    PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
        //    bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
        //    using (Stream stm = File.Create(fileName))
        //    {
        //        bitmapEncoder.Save(stm);
        //    }
        //}
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
        async void Generation()
        {
            Gen.Visibility = Visibility.Hidden;
            await Task.Delay(500);
            int countGen = 0;
            try
            {
                countGen = Int32.Parse(CountGen.Text);
            }
            catch
            {
                MessageBox.Show("Вводите только цифры в количество генераций");
            }
            bool error = false;
            while (countGen > 0)
            {
                countGen--;
                Worlds.Content = "";
                listWhiteBorder.Clear();
                foreach (Border border in listBorder)
                {
                    if (border.Background == Brushes.Transparent)
                    {
                        listWhiteBorder.Add(border);
                    }
                }
                foreach (var label in listLabel)
                {
                    label.Content = null;
                }
                foreach (Border whiteBorder in listWhiteBorder)
                {

                    int numberColumn = Grid.GetColumn(whiteBorder);
                    int numberRow = Grid.GetRow(whiteBorder);
                    bool black = true;
                    foreach (Border whiteLeftBorder in listWhiteBorder)
                    {
                        if (Grid.GetColumn(whiteLeftBorder) == numberColumn - 1 && Grid.GetRow(whiteLeftBorder) == numberRow)
                        {
                            black = false;
                            break;
                        }
                    }
                    if (black == true)
                    {
                        bool right = true;
                        error = classMove.InsertWord(right, whiteBorder, listWhiteBorder, listLabel, words, Worlds);
                        if (error)
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(1);
                        }
                    }



                    black = true;
                    foreach (Border whiteLeftBorder in listWhiteBorder)
                    {
                        if (Grid.GetColumn(whiteLeftBorder) == numberColumn && Grid.GetRow(whiteLeftBorder) == numberRow - 1)
                        {
                            black = false;
                            break;
                        }
                    }
                    if (black == true)
                    {
                        bool right = false;
                        error = classMove.InsertWord(right, whiteBorder, listWhiteBorder, listLabel, words, Worlds);
                        if (error)
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(1);
                        }
                    }
                }
                if (error == false)
                {

                    break;
                }
                else if (countGen == 0)
                {
                    Worlds.Content = "Ошибка в генерации";
                }
            }
            Gen.Visibility = Visibility.Visible;
        }
        private void MoveChangeColor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ChangeColorBlackWhite(sender);
            }
        }
        private void ClickChangeColor(object sender, MouseButtonEventArgs e)
        {
            ChangeColorBlackWhite(sender);
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
        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {
            Generation();
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