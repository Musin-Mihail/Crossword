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
    // Добавить кнопку СТОП
    // Установка цифр в начале слов. скриншоты заполненной и пустой сетки с определениями
    // Создать структуру которая будет ставить слово, потом перебирать все слова, если не подходит, менять предыдущее и пробовать снова
    public partial class MainWindow : Window
    {
        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();
        List<Border> listWhiteBorder = new List<Border>();
        List<Label> listWhiteLabel = new List<Label>();
        List<string> words;
        GenerationWord generationWord = new GenerationWord();
        List<Border> saveYellowListborder = new List<Border>();
        List<Border> saveWhiteListborder = new List<Border>();
        List<Word> ListWord = new List<Word>();
        public MainWindow()
        {
            InitializeComponent();
            string[] array = File.ReadAllLines("dict.txt");
            words = array.ToList();
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
        async void Generation()
        {
            Gen.Visibility = Visibility.Hidden;
            await Task.Delay(500);
            listWhiteBorder.Clear();
            ListWord.Clear();
            listWhiteLabel.Clear();
            foreach (Border border in listBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    listWhiteBorder.Add(border);
                    int column = Grid.GetColumn(border);
                    int row = Grid.GetRow(border);
                    foreach (var label in listLabel)
                    {
                        if (column == Grid.GetColumn(label))
                        {
                            if (row == Grid.GetRow(label))
                            {
                                listWhiteLabel.Add(label);
                            }
                        }
                    }
                }
            }
            foreach (var label in listLabel)
            {
                label.Content = null;
            }
            // Поиск начало и длинну всех слов
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
                    SaveWordRight(numberColumn, numberRow);
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
                    SaveWordDown(numberColumn, numberRow);
                }
            }
            //Подбор слов
            int countGen = 0;
            try
            {
                countGen = Int32.Parse(CountGen.Text);
            }
            catch
            {
                MessageBox.Show("Вводите только цифры в количество генераций");
            }
            for (int i = 0; i < ListWord.Count; i++)
            {
                Word newWord = ListWord[i];
                ListWord.RemoveAt(i);
                if (newWord.GetRight() == true)
                {
                    List<string> listString = new List<string>();
                    int letterCount = newWord.GetRightLetterCount();
                    foreach (string word in words)
                    {
                        if (word.Length == letterCount)
                        {
                            listString.Add(word);
                        }
                    }
                    newWord.AddWordsRight(listString);
                }
                if (newWord.GetDown() == true)
                {
                    List<string> listString = new List<string>();
                    int letterCount = newWord.GetDownLetterCount();
                    foreach (string word in words)
                    {
                        if (word.Length == letterCount)
                        {
                            listString.Add(word);
                        }
                    }
                    newWord.AddWordsDown(listString);
                }
                ListWord.Insert(i, newWord);
            }
            bool error = false;
            while (countGen > 0)
            {
                foreach (var label in listLabel)
                {
                    label.Content = null;
                }
                countGen--;
                WindowsText.Content = "";
                foreach (var emptyWord in ListWord)
                {
                    bool right = emptyWord.GetRight();
                    bool down = emptyWord.GetDown();
                    if (right == true)
                    {
                        error = generationWord.InsertWord(true, emptyWord.GetFirstLabel(), listWhiteBorder, listWhiteLabel, emptyWord.GetRightListWords(), WindowsText);
                    }
                    if (down == true && error == false)
                    {
                        error = generationWord.InsertWord(false, emptyWord.GetFirstLabel(), listWhiteBorder, listWhiteLabel, emptyWord.GetDownListWords(), WindowsText);
                    }
                    if (error)
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(1);
                    }
                }
                if (error == false)
                {
                    break;
                }
                else if (countGen == 0)
                {
                    WindowsText.Content = "Ошибка в генерации";
                }
            }
            Gen.Visibility = Visibility.Visible;
        }
        void SaveWordRight(int firstNumColumn, int firstNumRow)
        {
            int letterСount = 0;
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listWhiteBorder)
                {
                    if (Grid.GetRow(border) == firstNumRow && Grid.GetColumn(border) == i)
                    {
                        letterСount++;
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    break;
                }
            }
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumColumn; i < firstNumColumn + letterСount; i++)
            {
                foreach (Label label in listWhiteLabel)
                {
                    if (Grid.GetRow(label) == firstNumRow && Grid.GetColumn(label) == i)
                    {
                        newListLabel.Add(label);
                    }
                }
            }
            if (newListLabel.Count > 1)
            {
                bool match = false;
                for (int i = 0; i < ListWord.Count; i++)
                {
                    if (newListLabel[0] == ListWord[i].GetFirstLabel())
                    {
                        Word newWord = ListWord[i];
                        ListWord.RemoveAt(i);
                        newWord.SetListLabelRight(newListLabel);
                        newWord.ChangeRight();
                        ListWord.Insert(i, newWord);
                        match = true;
                        break;
                    }
                }
                if (match == false)
                {
                    Word newWord = new Word();
                    newWord.SetListLabelRight(newListLabel);
                    newWord.ChangeRight();
                    ListWord.Add(newWord);
                }
            }
        }
        void SaveWordDown(int firstNumColumn, int firstNumRow)
        {
            int letterСount = 0;
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listWhiteBorder)
                {
                    if (Grid.GetRow(border) == i && Grid.GetColumn(border) == firstNumColumn)
                    {
                        letterСount++;
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    break;
                }
            }
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumRow; i < firstNumRow + letterСount; i++)
            {
                foreach (Label label in listWhiteLabel)
                {
                    if (Grid.GetColumn(label) == firstNumColumn && Grid.GetRow(label) == i)
                    {
                        newListLabel.Add(label);
                    }
                }
            }
            if (newListLabel.Count > 1)
            {

                bool match = false;
                for (int i = 0; i < ListWord.Count; i++)
                {
                    if (newListLabel[0] == ListWord[i].GetFirstLabel())
                    {
                        Word newWord = ListWord[i];
                        ListWord.RemoveAt(i);
                        newWord.SetListLabelDown(newListLabel);
                        newWord.ChangeDown();
                        ListWord.Insert(i, newWord);
                        match = true;
                        break;
                    }
                }
                if (match == false)
                {
                    Word newWord = new Word();

                    newWord.SetListLabelDown(newListLabel);
                    newWord.ChangeDown();
                    ListWord.Add(newWord);
                }
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