using System;
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
        List<string> listWordsString = new List<string>();

        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();

        List<Word> listWord = new List<Word>();
        List<Border> listEmptyBorder = new List<Border>();
        List<Label> listEmptyLabel = new List<Label>();
        bool STOP = false;

        public MainWindow()
        {
            InitializeComponent();
            CreateDictionary();
            CreateUIGrid();
        }
        //Создание основы
        public void CreateDictionary()
        {
            string[] array = File.ReadAllLines("dict.txt");
            listWordsString = array.ToList();
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
        }
        private Border CreateBorder()
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.MouseEnter += new MouseEventHandler(MoveChangeColor);
            myBorder.MouseDown += new MouseButtonEventHandler(ClickChangeColor);
            return myBorder;
        }
        private void MoveChangeColor(object sender, MouseEventArgs e)
        {
            ChangeColorBlackWhite(sender);
        }
        private void ClickChangeColor(object sender, MouseButtonEventArgs e)
        {
            ChangeColorBlackWhite(sender);
        }
        Label CreateLabel()
        {
            Label myLabel = new Label();
            myLabel.HorizontalAlignment = HorizontalAlignment.Center;
            myLabel.VerticalAlignment = VerticalAlignment.Center;
            return myLabel;
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


        //Подбор слов
        private void Button_ClickGen(object sender, RoutedEventArgs e)
        {
            Generation();
        }
        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            STOP = true;
        }
        public void Generation()
        {
            GenButton.Visibility = Visibility.Hidden;
            GenStopButton.Visibility = Visibility.Visible;
            listWord.Clear();
            listEmptyBorder.Clear();
            listEmptyLabel.Clear();

            SearchForEmptyCells();

            SearchForTheBeginningAndLengthOfAllWords();

            SearchForConnectedWords();

            SearchForWordsByLength();

            SelectionAndInstallationOfWords();

            DisplayingWordsOnTheScreen();

            GenStopButton.Visibility = Visibility.Hidden;
            GenButton.Visibility = Visibility.Visible;
        }
        void SearchForEmptyCells()
        {
            foreach (Border border in listBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    listEmptyBorder.Add(border);
                    int column = Grid.GetColumn(border);
                    int row = Grid.GetRow(border);
                    foreach (var label in listLabel)
                    {
                        if (column == Grid.GetColumn(label))
                        {
                            if (row == Grid.GetRow(label))
                            {
                                listEmptyLabel.Add(label);
                            }
                        }
                    }
                }
            }
        }
        void SearchForTheBeginningAndLengthOfAllWords()
        {
            int count = 0;
            foreach (Border whiteBorder in listEmptyBorder)
            {
                int numberColumn = Grid.GetColumn(whiteBorder);
                int numberRow = Grid.GetRow(whiteBorder);
                bool black = true;

                foreach (Border whiteLeftBorder in listEmptyBorder)
                {
                    if (Grid.GetColumn(whiteLeftBorder) == numberColumn - 1 && Grid.GetRow(whiteLeftBorder) == numberRow)
                    {
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    SaveWordRight(ref count, numberColumn, numberRow);
                }
                black = true;
                foreach (Border whiteLeftBorder in listEmptyBorder)
                {
                    if (Grid.GetColumn(whiteLeftBorder) == numberColumn && Grid.GetRow(whiteLeftBorder) == numberRow - 1)
                    {
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    SaveWordDown(ref count, numberColumn, numberRow);
                }
            }
        }
        void SaveWordRight(ref int count, int firstNumColumn, int firstNumRow)
        {
            int letterСount = 0;
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listEmptyBorder)
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
                foreach (Label label in listEmptyLabel)
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
                    if (newListLabel[0] == listWord[i].firstLabel)
                    {
                        Word newWord = listWord[i];
                        listWord.RemoveAt(i);
                        newWord.SetListLabelRight(newListLabel);
                        count++;
                        newWord.count = count;
                        newWord.right = true;
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
                    newWord.count = count;
                    newWord.right = true;
                    listWord.Add(newWord);
                }
            }
        }
        void SaveWordDown(ref int count, int firstNumColumn, int firstNumRow)
        {
            int letterСount = 0;
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listEmptyBorder)
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
                foreach (Label label in listEmptyLabel)
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
                    if (newListLabel[0] == listWord[i].firstLabel)
                    {
                        Word newWord = listWord[i];
                        listWord.RemoveAt(i);
                        newWord.SetListLabelDown(newListLabel);
                        count++;
                        newWord.count = count;
                        newWord.down = true;
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
                    newWord.count = count;
                    newWord.down = true;
                    listWord.Add(newWord);
                }
            }
        }
        void SearchForConnectedWords()
        {
            if (listWord.Count > 0)
            {
                List<Word> listMatchWord = new List<Word>();
                List<Word> tempListWord = new List<Word>(listWord);
                Word word = tempListWord[0];
                listMatchWord.Add(word);
                tempListWord.RemoveAt(0);
                SearchMatch(ref listMatchWord, ref tempListWord, word);
                listWord = listMatchWord;
            }
        }
        void SearchMatch(ref List<Word> listMatchWord, ref List<Word> tempListWord, Word word)
        {
            if (word.right == true)
            {
                List<Label> tempListLabel = word.listLabelRight;
                foreach (Label label in tempListLabel)
                {
                    int listCount = tempListWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (tempListWord[i].down == true)
                        {
                            bool match = tempListWord[i].SearchForMatchesDown(label);
                            if (match)
                            {
                                //word.AddConnectionWordsDown(listWord[i]);
                                if (listMatchWord.Contains(tempListWord[i]) == false)
                                {
                                    listMatchWord.Add(tempListWord[i]);
                                    Word newWord = tempListWord[i];
                                    int index = tempListWord.IndexOf(tempListWord[i]);
                                    tempListWord.RemoveAt(index);
                                    if (tempListWord.Count > 0)
                                    {
                                        SearchMatch(ref listMatchWord, ref tempListWord, newWord);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (word.down == true)
            {
                List<Label> tempListLabel = word.listLabelDown;
                foreach (Label label in tempListLabel)
                {
                    int listCount = tempListWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (tempListWord[i].right == true)
                        {
                            bool match = tempListWord[i].SearchForMatchesRight(label);
                            if (match)
                            {
                                //word.AddConnectionWordsRight(listWord[i]);
                                if (listMatchWord.Contains(tempListWord[i]) == false)
                                {
                                    listMatchWord.Add(tempListWord[i]);
                                    Word newWord = tempListWord[i];
                                    int index = tempListWord.IndexOf(tempListWord[i]);
                                    tempListWord.RemoveAt(index);
                                    if (tempListWord.Count > 0)
                                    {
                                        SearchMatch(ref listMatchWord, ref tempListWord, newWord);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        void SearchForWordsByLength()
        {
            for (int i = 0; i < listWord.Count; i++)
            {
                Word newWord = listWord[i];
                listWord.RemoveAt(i);
                if (newWord.right == true)
                {
                    List<string> listString = new List<string>();
                    int letterCount = newWord.listLabelRight.Count;
                    foreach (string word in listWordsString)
                    {
                        if (word.Length == letterCount)
                        {
                            listString.Add(word);
                        }
                    }
                    newWord.AddWordsRight(listString);
                }
                if (newWord.down == true)
                {
                    List<string> listString = new List<string>();
                    int letterCount = newWord.listLabelDown.Count;
                    foreach (string word in listWordsString)
                    {
                        if (word.Length == letterCount)
                        {
                            listString.Add(word);
                        }
                    }
                    newWord.AddWordsDown(listString);
                }
                listWord.Insert(i, newWord);
            }
            foreach (var word in listWord)
            {
                word.ListWordsRandomization();
            }
        }
        void SelectionAndInstallationOfWords()
        {
            foreach (var label in listLabel)
            {
                label.Content = null;
            }
            NewGen2(0);
        }
        void NewGen2(int index)
        {
            if (index < listWord.Count)
            {
                Label FirsLabel = listWord[index].firstLabel;
                FirsLabel.Background = Brushes.Yellow;

                Word newWord = listWord[index];
                int error = InsertWord(listEmptyBorder, listEmptyLabel, ref newWord);
                FirsLabel.Background = Brushes.White;

                if (error == 0)
                {
                    NewGen2(index + 1);
                }
                else
                {
                    // Программа зависает, если пересечение идёт не с предыдущим словом а со словом которое было раньше.
                    // Нужно в слова добавить слова с пересечением. И обращаться к ним напрямую.
                    if (index > 0)
                    {
                        Word newWord2 = listWord[index];
                        listWord.RemoveAt(index);
                        newWord2.ClearLabelRight();
                        newWord2.ClearLabelDown();
                        newWord2.RestoreDictionary();
                        listWord.Insert(index, newWord2);

                        if (error == 1)
                        {
                            MessageBox.Show(error + " 1");
                            List<Word> newListWord = newWord2.ConnectionWordsRight;
                            foreach (Word word in newListWord)
                            {
                                int index2 = listWord.IndexOf(word);
                                newWord2 = listWord[index2];
                                listWord.RemoveAt(index2);
                                //newWord2.ClearLabelRight();
                                newWord2.ClearLabelDown();
                                listWord.Insert(index2, newWord2);
                            }

                        }
                        if (error == 2)
                        {
                            MessageBox.Show(error + " 2");
                            List<Word> newListWord = newWord2.ConnectionWordsDown;
                            foreach (Word word in newListWord)
                            {
                                //int index2 = ListWord.IndexOf(word);
                                //newWord2 = ListWord[index2];
                                //ListWord.RemoveAt(index2);
                                word.ClearLabelRight();
                                //newWord2.ClearLabelDown();
                                //ListWord.Insert(index2, newWord2);
                            }

                        }
                        NewGen2(index - 1);
                    }
                    else
                    {
                        Word newWord2 = listWord[index];
                        listWord.RemoveAt(index);
                        newWord2.ClearLabelRight();
                        newWord2.ClearLabelDown();
                        //newWord2.RestoreDictionary();
                        listWord.Insert(index, newWord2);
                        NewGen2(index);
                        MessageBox.Show(error + " 0");
                    }
                }

            }
        }
        int InsertWord(List<Border> listBorder, List<Label> listLabel, ref Word word)
        {
            bool right = word.right;
            bool down = word.down;

            bool errorRight = false;
            bool errorDown = false;

            Label FirsLabel = word.firstLabel;
            int numColumn = Grid.GetColumn(FirsLabel);
            int numRow = Grid.GetRow(FirsLabel);

            if (right == true)
            {
                MessageBox.Show("Вправо");
                List<string> words = word.listTempWordsRight;
                List<Label> newListLabelRight = SearchEmptyLineRight(numColumn, numRow, listBorder, listLabel);
                errorRight = SearchWord(true, newListLabelRight, words, ref word);
            }
            if (down == true)
            {
                MessageBox.Show("Вниз");
                List<string> words = word.listTempWordsDown;
                List<Label> newListLabelDown = SearchEmptyLineDown(numColumn, numRow, listBorder, listLabel);
                errorDown = SearchWord(false, newListLabelDown, words, ref word);
            }
            if (errorRight == true && errorDown == true)
            {
                return 3;
            }
            else if (errorRight == true)
            {
                return 1;
            }
            else if (errorDown == true)
            {
                return 2;
            }
            return 0;
            // 0 - нет ошибок
            // 1 - ошибка в вправа
            // 2 - ошибка во влево
            // 3 - ошибка в обоих словах
        }
        List<Label> SearchEmptyLineRight(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            //Поиск место под слово по горизонтали. И возрат списка клеток.
            int letterСount = 0;
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listBorder)
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
                foreach (Label label in listLabel)
                {
                    if (Grid.GetRow(label) == firstNumRow && Grid.GetColumn(label) == i)
                    {
                        newListLabel.Add(label);
                    }
                }
            }
            return newListLabel;
        }
        List<Label> SearchEmptyLineDown(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            //Поиск место под слово по вертикали. И возрат списка клеток.
            int letterСount = 0;
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listBorder)
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
                foreach (Label label in listLabel)
                {
                    if (Grid.GetColumn(label) == firstNumColumn && Grid.GetRow(label) == i)
                    {
                        newListLabel.Add(label);
                    }
                }
            }
            return newListLabel;
        }
        bool SearchWord(bool right, List<Label> newListLabel, List<string> words, ref Word word)
        {
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count > 1)
                {
                    List<string> listWordsString = new List<string>(words);
                    List<string> tempListString = new List<string>();
                    if (right == true)
                    {
                        word.ConnectionPointRight.Clear();
                    }
                    else
                    {
                        word.ConnectionPointDown.Clear();
                    }
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {
                            // Нужно добавить эти точки в обо слова. А лучше добавить сразу все слова.
                            if (right == true)
                            {
                                word.ConnectionPointRight.Add(newListLabel[i]);
                            }
                            else
                            {
                                word.ConnectionPointDown.Add(newListLabel[i]);
                            }
                            foreach (string item in listWordsString)
                            {
                                string tempString = newListLabel[i].Content.ToString();
                                string tempString2 = item[i].ToString();
                                if (tempString2 == tempString)
                                {
                                    tempListString.Add(item);
                                }
                            }
                            if (tempListString.Count > 0)
                            {
                                listWordsString = new List<string>(tempListString);
                                tempListString.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Не нашёл слово");
                                return true;
                            }
                        }
                    }

                    string newWord = listWordsString[0];
                    if (right == true)
                    {
                        word.DeleteWordRight(newWord);
                    }
                    else
                    {
                        word.DeleteWordDown(newWord);
                    }
                    MessageBox.Show(newWord);
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        newListLabel[i].Content = newWord[i];
                    }
                }
            }
            else
            {
                MessageBox.Show("Есть поле больше 15");
            }
            return false;
        }
        void DisplayingWordsOnTheScreen()
        {
            WindowsText.Content = "";
            WindowsText.Content += "По горизонтали\n";
            string newText = "";
            // Есть пустые слова. Нужно найти как они добавляються
            foreach (var word in listWord)
            {
                var test = word.listLabelRight;
                if (test.Count > 1)
                {
                    foreach (var label in test)
                    {
                        if (label.Content != null)
                        {
                            newText = label.Content.ToString();
                            if (newText.Length == 1)
                            {
                                WindowsText.Content += label.Content.ToString();
                            }
                        }
                    }
                    WindowsText.Content += "\n";
                }
            }
            WindowsText.Content += "\nПо вертикали\n";
            foreach (var word in listWord)
            {
                var test = word.listLabelDown;
                if (test.Count > 1)
                {
                    foreach (var label in test)
                    {
                        if (label.Content != null)
                        {
                            newText = label.Content.ToString();
                            if (newText.Length == 1)
                            {
                                WindowsText.Content += label.Content.ToString();
                            }
                        }
                    }
                    WindowsText.Content += "\n";
                }
            }
        }


        //Сохраненние и загрузка
        private void Button_ClickSaveGrid(object sender, RoutedEventArgs e)
        {
            Save();
            SaveGrid.Content = "Сетка сохранена";
        }
        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            Load();
            SaveGrid.Content = "Сетка загружена";
        }
        public void Save()
        {
            string saveFile = "";
            foreach (var border in listBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    int Column = Grid.GetColumn(border);
                    int Row = Grid.GetRow(border);
                    saveFile += Column + ";" + Row + "\n";
                }
            }
            File.WriteAllText("SaveGrid.txt", saveFile);
        }
        public void Load()
        {
            foreach (var label in listLabel)
            {
                label.Content = "";
            }
            foreach (var border in listBorder)
            {
                border.Background = Brushes.Black;
            }
            var test = File.ReadAllLines("SaveGrid.txt");
            foreach (var item in test)
            {
                List<string> strings = new List<string>(item.Split(';'));
                int Column = Int32.Parse(strings[0]);
                int Row = Int32.Parse(strings[1]);
                foreach (var item2 in listBorder)
                {
                    if (Grid.GetColumn(item2) == Column)
                    {
                        if (Grid.GetRow(item2) == Row)
                        {
                            item2.Background = Brushes.Transparent;
                        }
                    }
                }
            }
        }
    }
}
