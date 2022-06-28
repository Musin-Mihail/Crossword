using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
namespace Crossword
{
    public partial class MainWindow : Window
    {
        List<List<string>> listWordsList = new List<List<string>>();

        int cellSize = 30;
        int numberOfCellsHorizontally = 30;
        int numberOfCellsVertically = 30;

        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();

        List<Word> listWordStruct = new List<Word>();
        bool STOP = false;

        public MainWindow()
        {
            InitializeComponent();
            AddingMatermarks();
            CreateDictionary();
            CreateUIGrid();
        }

        //Создание основы
        void AddingMatermarks()
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
        public void CreateDictionary()
        {
            string[] array = File.ReadAllLines("dict.txt");
            List<string> listWordsString = array.ToList();
            for (int i = 0; i < 20; i++)
            {
                List<string> list = new List<string>();
                listWordsList.Add(list);
            }
            foreach (string word in listWordsString)
            {
                listWordsList[word.Length].Add(word);
            }
        }
        void CreateUIGrid()
        {
            for (int x = 0; x < numberOfCellsHorizontally; x++)
            {
                for (int y = 0; y < numberOfCellsVertically; y++)
                {
                    Cell cell = new Cell();
                    Border border = CreateBorder(x, y);
                    TheGrid.Children.Add(border);
                    Label label = CreateLabel();
                    //label.HorizontalAlignment = HorizontalAlignment.Center;
                    //label.VerticalAlignment = VerticalAlignment.Top;
                    label.FontSize = 20;
                    border.Child = label;
                    label.Margin = new Thickness(0, -4, 0, 0);
                    cell.AddBorderLabelXY(border, label, x, y);
                    listAllCellStruct.Add(cell);
                }
            }
        }
        private Border CreateBorder(int x, int y)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.MouseEnter += new MouseEventHandler(MoveChangeColor);
            myBorder.MouseDown += new MouseButtonEventHandler(ClickChangeColor);
            myBorder.Margin = new Thickness(x * cellSize, y * cellSize, 0, 0);
            myBorder.Width = cellSize;
            myBorder.Height = cellSize;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            return myBorder;
        }
        int GetXCellBorder(Border border)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                if (border == cell.border)
                {
                    return cell.x;
                }
            }
            return -1;
        }
        int GetYCellBorder(Border border)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                if (border == cell.border)
                {
                    return cell.y;
                }
            }
            return -1;
        }
        int GetXCellLabel(Label label)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                if (label == cell.label)
                {
                    return cell.x;
                }
            }
            return -1;
        }
        int GetYCellLabel(Label label)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                if (label == cell.label)
                {
                    return cell.y;
                }
            }
            return -1;
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
            listWordStruct.Clear();
            listEmptyCellStruct.Clear();

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
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.border.Background == Brushes.Transparent)
                {
                    listEmptyCellStruct.Add(cell);
                }
            }
        }
        void SearchForTheBeginningAndLengthOfAllWords()
        {
            int count = 0;
            foreach (Cell cell in listEmptyCellStruct)
            {
                int x = cell.x;
                int y = cell.y;
                bool black = true;

                foreach (Cell cell2 in listEmptyCellStruct)
                {
                    if (cell2.x == x - 1 && cell2.y == y)
                    {
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    SaveWordRight(ref count, x, y);
                }
                black = true;
                foreach (Cell cell2 in listEmptyCellStruct)
                {
                    if (cell2.x == x && cell2.y == y - 1)
                    {
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    SaveWordDown(ref count, x, y);
                }
            }
        }
        void SaveWordRight(ref int count, int firstNumColumn, int firstNumRow)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == firstNumRow && cell.x == i)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            break;
                        }
                    }
                }
                if (black == true)
                {
                    break;
                }
            }

            if (newListLabel.Count > 1)
            {
                bool match = false;
                for (int i = 0; i < listWordStruct.Count; i++)
                {
                    if (newListLabel[0] == listWordStruct[i].firstLabel)
                    {
                        Word newWord = listWordStruct[i];
                        newWord.SetListLabelRight(newListLabel);
                        count++;
                        newWord.count = count;
                        newWord.right = true;
                        RefreshWord(i, newWord);
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
                    listWordStruct.Add(newWord);
                }
            }
        }
        void SaveWordDown(ref int count, int firstNumColumn, int firstNumRow)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == i && cell.x == firstNumColumn)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            break;
                        }
                    }
                }
                if (black == true)
                {
                    break;
                }
            }

            if (newListLabel.Count > 1)
            {
                bool match = false;
                for (int i = 0; i < listWordStruct.Count; i++)
                {
                    if (newListLabel[0] == listWordStruct[i].firstLabel)
                    {
                        Word newWord = listWordStruct[i];
                        newWord.SetListLabelDown(newListLabel);
                        count++;
                        newWord.count = count;
                        newWord.down = true;
                        RefreshWord(i, newWord);
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
                    listWordStruct.Add(newWord);
                }
            }
        }
        void SearchForConnectedWords()
        {
            if (listWordStruct.Count > 0)
            {
                List<Word> listMatchWord = new List<Word>();
                List<Word> tempListWord = new List<Word>();
                //первым добавить самое сложное слово.
                tempListWord.Add(listWordStruct[0]);
                while (tempListWord.Count > 0)
                {
                    Word word = tempListWord[0];
                    //listMatchWord.Add(word);
                    tempListWord.RemoveAt(0);
                    if (word.right == true)
                    {
                        List<Label> tempListLabel = word.listLabelRight;
                        foreach (Label label in tempListLabel)
                        {
                            foreach (var word2 in listWordStruct)
                            {
                                if (word.count != word2.count)
                                {
                                    if (word2.SearchForMatchesDown(label) == true)
                                    {
                                        if (listMatchWord.Contains(word2) == false)
                                        {
                                            tempListWord.Add(word2);
                                            listMatchWord.Add(word2);
                                            break;
                                        }
                                        break;
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
                            foreach (var word2 in listWordStruct)
                            {
                                if (word.count != word2.count)
                                {
                                    if (word2.SearchForMatchesRight(label) == true)
                                    {
                                        if (listMatchWord.Contains(word2) == false)
                                        {
                                            tempListWord.Add(word2);
                                            listMatchWord.Add(word2);
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                listWordStruct = listMatchWord;
                //foreach (var item in listWordStruct)
                //{
                //    foreach (var item2 in listWordStruct)
                //    {
                //        if(item.count == item2.count)
                //        {
                //            MessageBox.Show(item.count +" - "+ item2.count);
                //        }
                //    }
                //}
                //foreach (var item in listWordStruct)
                //{
                //    if (item.right)
                //    {
                //        foreach (var item2 in item.listLabelRight)
                //        {
                //            item2.Background = Brushes.Yellow;
                //        }
                //        MessageBox.Show("listLabelRight");
                //        foreach (var item2 in item.listLabelRight)
                //        {
                //            item2.Background = Brushes.Red;
                //        }
                //    }
                //    if (item.down)
                //    {
                //        foreach (var item2 in item.listLabelDown)
                //        {
                //            item2.Background = Brushes.Yellow;
                //        }
                //        MessageBox.Show("listLabelDown");
                //        foreach (var item2 in item.listLabelDown)
                //        {
                //            item2.Background = Brushes.Red;
                //        }
                //    }
                //}
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
            for (int i = 0; i < listWordStruct.Count; i++)
            {
                Word newWord = listWordStruct[i];
                if (newWord.right == true)
                {
                    int letterCount = newWord.listLabelRight.Count;
                    newWord.AddWordsRight(listWordsList[letterCount]);
                }
                if (newWord.down == true)
                {
                    int letterCount = newWord.listLabelDown.Count;
                    newWord.AddWordsDown(listWordsList[letterCount]);
                }
                RefreshWord(i, newWord);
            }
            foreach (var word in listWordStruct)
            {
                word.ListWordsRandomization();
            }
        }
        void SelectionAndInstallationOfWords()
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
            }
            NewGen2(0);
        }
        void NewGen2(int index)
        {
            WindowsText.Content = "";
            int stop = index;
            int count = 0;
            int countGen = 0;
            int maxCountGen = 0;
            int maxCountWord = 0;
            try
            {
                maxCountWord = Int32.Parse(CountGenWord.Text);
            }
            catch
            {
                MessageBox.Show("Введите цифры в количество попыток для слова");
                return;
            }

            try
            {
                maxCountGen = Int32.Parse(CountGen.Text);
            }
            catch
            {
                MessageBox.Show("Введите цифры в количество попыток для всей генерации");
                return;
            }

            while (index < listWordStruct.Count)
            {
                count++;
                Word newWord = listWordStruct[index];
                int error = InsertWord(ref newWord);
                if (count > maxCountWord)
                {
                    if (countGen < maxCountGen)
                    {
                        MessageBox.Show("");
                        for (int i = 0; i < listWordStruct.Count; i++)
                        {
                            Word newWord2 = listWordStruct[i];
                            newWord2.Reset();
                            RefreshWord(i, newWord2);
                        }
                        countGen++;
                        index = 0;
                        count = 0;
                        stop = 0;
                        
                        continue;
                    }
                    else
                    {
                        WindowsText.Content = "ОШИБКА ГЕНЕРАЦИИ\nОШИБКА ГЕНЕРАЦИИ\nОШИБКА ГЕНЕРАЦИИ\n";
                        break;
                    }
                }
                if (index > stop)
                {
                    count = 0;
                    stop = index;
                }

                if (error == 0)
                {
                    index++;
                    continue;
                }
                else
                {
                    if (index > 0)
                    {
                        listWordStruct[index].RefreshListLabelRight();
                        listWordStruct[index].RefreshListLabelDown();
                        listWordStruct[index].ClearLabelRight();
                        listWordStruct[index].ClearLabelDown();
                        listWordStruct[index].RestoreDictionary();
                        RefreshWord(index, listWordStruct[index]);

                        listWordStruct[index - 1].ClearLabelDown();
                        listWordStruct[index - 1].ClearLabelRight();
                        index--;
                        continue;
                    }
                    else
                    {
                        listWordStruct[index].ClearLabelRight();
                        listWordStruct[index].ClearLabelDown();
                        RefreshWord(index, listWordStruct[index]);
                        continue;
                    }
                }
            }
        }
        int InsertWord(ref Word word)
        {
            bool right = word.right;
            bool down = word.down;

            bool errorRight = false;
            bool errorDown = false;

            Label firstLabel = word.firstLabel;
            int x = GetXCellLabel(firstLabel);
            int y = GetYCellLabel(firstLabel);

            if (right == true)
            {
                List<string> words = word.listTempWordsRight;
                if (words.Count == 0)
                {
                    errorRight = true;
                }
                else
                {
                    List<Label> newListLabelRight = SearchEmptyLineRight(x, y);
                    errorRight = SearchWord(true, newListLabelRight, words, ref word);
                }
            }
            if (down == true)
            {
                List<string> words = word.listTempWordsDown;
                if (words.Count == 0)
                {
                    errorRight = true;
                }
                else
                {
                    List<Label> newListLabelDown = SearchEmptyLineDown(x, y);
                    errorDown = SearchWord(false, newListLabelDown, words, ref word);
                }
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
        List<Label> SearchEmptyLineRight(int firstNumColumn, int firstNumRow)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == firstNumRow && cell.x == i)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            //cell.border.Background = Brushes.Yellow;
                            //MessageBox.Show("");
                            break;
                        }
                    }
                }
                if (black == true)
                {
                    break;
                }
            }
            return newListLabel;
        }
        List<Label> SearchEmptyLineDown(int firstNumColumn, int firstNumRow)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == i && cell.x == firstNumColumn)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            break;
                        }
                    }
                }
                if (black == true)
                {
                    break;
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
                                return true;
                            }
                        }
                    }
                    string newWord = listWordsString[0];
                    word.DeleteWord(newWord);
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
            WindowsText.Content += "По горизонтали\n";
            string newText = "";
            foreach (var word in listWordStruct)
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
            foreach (var word in listWordStruct)
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
        void RefreshWord(int index, Word word)
        {
            listWordStruct.RemoveAt(index);
            listWordStruct.Insert(index, word);
        }

        //Сохранение и загрузка
        private void Button_ClickSaveGrid(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void Button_ClickLoadGrid(object sender, RoutedEventArgs e)
        {
            Load();
        }
        public void Save()
        {
            string saveFile = "";
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.border.Background == Brushes.Transparent)
                {
                    saveFile += cell.x + ";" + cell.y + "\n";
                }
            }
            File.WriteAllText("SaveGrid.txt", saveFile);
        }
        public void Load()
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
            }
            foreach (Cell cell in listAllCellStruct)
            {
                cell.border.Background = Brushes.Black;
            }
            var test = File.ReadAllLines("SaveGrid.txt");
            foreach (var item in test)
            {
                List<string> strings = new List<string>(item.Split(';'));
                int x = Int32.Parse(strings[0]);
                int y = Int32.Parse(strings[1]);
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.x == x)
                    {
                        if (cell.y == y)
                        {
                            cell.border.Background = Brushes.Transparent;
                        }
                    }
                }
            }
        }
    }
}