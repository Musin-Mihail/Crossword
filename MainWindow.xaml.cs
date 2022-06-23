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
        List<Label> listWhiteLabel = new List<Label>();
        List<string> words;
        GenerationWord generationWord = new GenerationWord();
        List<Border> saveYellowListborder = new List<Border>();
        List<Border> saveWhiteListborder = new List<Border>();
        bool STOP = false;
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
            if (Visual.IsChecked == true)
            {
                GenStop.Visibility = Visibility.Visible;
            }
            await Task.Delay(100);
            listWhiteBorder.Clear();
            List<Word> ListWord = new List<Word>();
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
            int count = 0;
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
                    SaveWordRight(ref ListWord, ref count, numberColumn, numberRow);

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
                    SaveWordDown(ref ListWord, ref count, numberColumn, numberRow);

                }
            }
            //Поиск соединённых слов
            List<Word> listMatchWord = new List<Word>();
            List<Word> tempListWord = new List<Word>(ListWord);
            Word newWord2 = tempListWord[0];
            listMatchWord.Add(newWord2);
            tempListWord.RemoveAt(0);
            SearchMatch(ref listMatchWord, ref tempListWord, newWord2);
            ListWord = listMatchWord;

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
            int allGen = countGen;
            while (countGen > 0)
            {
                if (STOP == true)
                {
                    STOP = false;
                    WindowsText.Content = "Генерация остановлена";
                    break;
                }
                bool error = false;
                CountGoodGen.Content = (allGen - countGen).ToString();
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
                    else if (Visual.IsChecked == true)
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

            CountGoodGen.Content = (allGen - countGen) + " Попыток";
            GenStop.Visibility = Visibility.Hidden;
            Gen.Visibility = Visibility.Visible;
        }
        void SearchMatch(ref List<Word> listMatchWord, ref List<Word> listWord, Word word)
        {
            if (word.GetRight() == true)
            {
                List<Label> tempListLabel = word.GetRightLabel();
                foreach (Label label in tempListLabel)
                {
                    int listCount = listWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (listWord[i].GetDown() == true)
                        {
                            bool match = listWord[i].SearchForMatchesDown(label);
                            if (match)
                            {
                                if (listMatchWord.Contains(listWord[i]) == false)
                                {
                                    listMatchWord.Add(listWord[i]);
                                    Word newWord = listWord[i];
                                    int index = listWord.IndexOf(listWord[i]);
                                    listWord.RemoveAt(index);
                                    if (listWord.Count > 0)
                                    {
                                        SearchMatch(ref listMatchWord, ref listWord, newWord);
                                    }
                                    break;
                                    //i++;
                                }
                            }
                        }
                    }
                }
            }
            if (word.GetDown() == true)
            {
                List<Label> tempListLabel = word.GetDownLabel();
                foreach (Label label in tempListLabel)
                {
                    int listCount = listWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (listWord[i].GetRight() == true)
                        {
                            bool match = listWord[i].SearchForMatchesRight(label);
                            if (match)
                            {
                                if (listMatchWord.Contains(listWord[i]) == false)
                                {
                                    listMatchWord.Add(listWord[i]);
                                    Word newWord = listWord[i];
                                    int index = listWord.IndexOf(listWord[i]);
                                    listWord.RemoveAt(index);
                                    if (listWord.Count > 0)
                                    {
                                        SearchMatch(ref listMatchWord, ref listWord, newWord);
                                    }
                                    break;
                                    //i++;
                                }
                            }
                        }
                    }
                }
            }
        }
        void SaveWordRight(ref List<Word> listWord, ref int count, int firstNumColumn, int firstNumRow)
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
                for (int i = 0; i < listWord.Count; i++)
                {
                    if (newListLabel[0] == listWord[i].GetFirstLabel())
                    {
                        Word newWord = listWord[i];
                        listWord.RemoveAt(i);
                        newWord.SetListLabelRight(newListLabel);
                        count++;
                        newWord.SetCount(count);
                        newWord.ChangeRight();
                        listWord.Insert(i, newWord);
                        match = true;
                        break;
                    }
                }
                if (match == false)
                {
                    Word newWord = new Word();
                    newWord.SetListLabelRight(newListLabel);
                    count++;
                    newWord.SetCount(count);
                    newWord.ChangeRight();
                    listWord.Add(newWord);
                }
            }
        }
        void SaveWordDown(ref List<Word> listWord, ref int count, int firstNumColumn, int firstNumRow)
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
                for (int i = 0; i < listWord.Count; i++)
                {
                    if (newListLabel[0] == listWord[i].GetFirstLabel())
                    {
                        Word newWord = listWord[i];
                        listWord.RemoveAt(i);
                        newWord.SetListLabelDown(newListLabel);
                        count++;
                        newWord.SetCount(count);
                        newWord.ChangeDown();
                        listWord.Insert(i, newWord);
                        match = true;
                        break;
                    }
                }
                if (match == false)
                {
                    Word newWord = new Word();
                    newWord.SetListLabelDown(newListLabel);
                    count++;
                    newWord.SetCount(count);
                    newWord.ChangeDown();
                    listWord.Add(newWord);
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

        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            STOP = true;
        }
    }
}