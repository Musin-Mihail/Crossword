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
            DefiningTheGenerationQueue();

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
                    SaveWordRight(x, y);
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
                    SaveWordDown(x, y);
                }
            }
        }
        void SaveWordRight(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = x; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == y && cell.x == i)
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
                Word newWord = new Word();
                newWord.SetListLabel(newListLabel);
                listWordStruct.Add(newWord);
            }
        }
        void SaveWordDown(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = y; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == i && cell.x == x)
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
                Word newWord = new Word();
                newWord.SetListLabel(newListLabel);
                listWordStruct.Add(newWord);
            }
        }
        void SearchForConnectedWords()
        {
            if (listWordStruct.Count > 0)
            {
                for (int i = 0; i < listWordStruct.Count; i++)
                {
                    Word word = listWordStruct[i];
                    List<Label> tempListLabel = word.listLabel;
                    int count = 0;
                    foreach (Label label in tempListLabel)
                    {
                        foreach (var word2 in listWordStruct)
                        {
                            if (word.listLabel != word2.listLabel && word2.SearchForMatches(label) == true)
                            {
                                count++;
                            }
                        }
                    }
                    word.difficulty = (float)count / tempListLabel.Count;
                    RefreshWord(i, word);
                }
                for (int i = 0; i < listWordStruct.Count; i++)
                {
                    Word word = listWordStruct[i];
                    List<Label> tempListLabel = word.listLabel;
                    foreach (Label label in tempListLabel)
                    {
                        foreach (var word2 in listWordStruct)
                        {
                            if (word.listLabel != word2.listLabel && word2.SearchForMatches(label) == true)
                            {
                                word.ConnectionWords.Add(word2);
                            }
                        }
                    }
                }
            }
        }
        void DefiningTheGenerationQueue()
        {
            if (listWordStruct.Count > 0)
            {
                Word theHardestWord = FindingTheHardestWord(listWordStruct);

                List<Word> listMatchWord = new List<Word>();
                List<Word> listAllMatchWord = new List<Word>();
                List<Word> listAllHardestWord = new List<Word>();
                List<Word> tempListWord = new List<Word>();
                List<Word> allRightWord = new List<Word>();

                listAllHardestWord.Add(theHardestWord);
                listAllMatchWord.Add(theHardestWord);

                tempListWord.Add(theHardestWord);

                while (tempListWord.Count > 0)
                {
                    Word word = tempListWord[0];
                    tempListWord.RemoveAt(0);

                    if (allRightWord.Contains(word) == false)
                    {
                        allRightWord.Add(word);
                        foreach (Word word2 in word.ConnectionWords)
                        {
                            bool match = false;
                            foreach (Word word3 in listAllMatchWord)
                            {
                                if (word2.listLabel == word3.listLabel)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (match == false)
                            {
                                foreach (Word word3 in listMatchWord)
                                {
                                    if (word2.listLabel == word3.listLabel)
                                    {
                                        match = true;
                                        break;
                                    }
                                }
                            }
                            if (match == false)
                            {
                                listAllMatchWord.Add(word2);
                                listMatchWord.Add(word2);
                            }
                        }
                    }

                    if (listMatchWord.Count > 0)
                    {
                        theHardestWord = FindingTheHardestWord(listMatchWord);

                        int index = listMatchWord.IndexOf(theHardestWord);
                        listMatchWord.RemoveAt(index);

                        tempListWord.Add(theHardestWord);
                        listAllHardestWord.Add(theHardestWord);
                    }

                }
                listWordStruct = listAllHardestWord;

                //НЕ УДАЛЯТЬ
                //foreach (var item in listWordStruct)
                //{
                //    foreach (var item2 in item.listLabel)
                //    {
                //        item2.Background = Brushes.Yellow;
                //    }
                //    MessageBox.Show(item.difficulty + " listLabelRight");
                //    foreach (var item2 in item.listLabel)
                //    {
                //        item2.Background = Brushes.Red;
                //    }
                //}
            }
        }
        Word FindingTheHardestWord(List<Word> listWords)
        {
            Word theHardestWord = listWords[0];
            float maxDifficulty = 0;
            int maxWordCount = 0;
            foreach (Word word in listWords)
            {
                if (word.difficulty >= maxDifficulty)
                {
                    maxDifficulty = word.difficulty;
                    if (word.listLabel.Count > maxWordCount)
                    {
                        maxWordCount = word.listLabel.Count;
                        theHardestWord = word;
                    }
                }
            }
            return theHardestWord;
        }
        void SearchForWordsByLength()
        {
            for (int i = 0; i < listWordStruct.Count; i++)
            {
                Word newWord = listWordStruct[i];
                int letterCount = newWord.listLabel.Count;
                newWord.AddWords(listWordsList[letterCount]);
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
                        listWordStruct[index].RefreshListLabel();
                        listWordStruct[index].ClearLabel();
                        listWordStruct[index].RestoreDictionary();
                        RefreshWord(index, listWordStruct[index]);

                        listWordStruct[index - 1].ClearLabel();
                        index--;
                        continue;
                    }
                    else
                    {
                        listWordStruct[index].ClearLabel();
                        RefreshWord(index, listWordStruct[index]);
                        continue;
                    }
                }
            }
        }
        int InsertWord(ref Word word)
        {

            bool error = false;

            Label firstLabel = word.firstLabel;
            int x = GetXCellLabel(firstLabel);
            int y = GetYCellLabel(firstLabel);

            List<string> words = word.listTempWords;
            if (words.Count == 0)
            {
                error = true;
            }
            else
            {
                List<Label> newListLabelRight = SearchEmptyLineRight(x, y);
                if (newListLabelRight.Count > 0)
                {
                    error = SearchWord(newListLabelRight, words, ref word);
                }
                if (error == false)
                {
                    List<Label> newListLabelDown = SearchEmptyLineDown(x, y);
                    if (newListLabelDown.Count > 0)
                    {
                        error = SearchWord(newListLabelDown, words, ref word);
                    }
                }
            }

            if (error == true)
            {
                return 1;
            }
            return 0;
            // 0 - нет ошибок
            // 1 - ошибка в вправа
            // 2 - ошибка во влево
            // 3 - ошибка в обоих словах
        }
        List<Label> SearchEmptyLineRight(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.y == y - 1 && cell.x == x)
                {
                    if (cell.border.Background == Brushes.Black)
                    {
                        return newListLabel;
                    }
                }
            }
            for (int i = x; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == y && cell.x == i)
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
        List<Label> SearchEmptyLineDown(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.y == y && cell.x == x - 1)
                {
                    if (cell.border.Background == Brushes.Black)
                    {
                        return newListLabel;
                    }
                }
            }
            for (int i = y; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == i && cell.x == x)
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
        bool SearchWord(List<Label> newListLabel, List<string> words, ref Word word)
        {
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count > 1)
                {
                    string test = words[0];
                    List<string> listWordsString = new List<string>(words);
                    List<string> tempListString = new List<string>();

                    word.ConnectionLabel.Clear();

                    //Поиск слов с теми же буквами
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {

                            word.ConnectionLabel.Add(newListLabel[i]);

                            string tempString = newListLabel[i].Content.ToString();
                            foreach (string item in listWordsString)
                            {
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
                                //Если нет подходящего слова
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
                    MessageBox.Show(newWord);
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
                var test = word.listLabel;
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