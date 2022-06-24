using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Linq;


namespace Crossword
{
    internal class GridGeneration
    {
        GenerationWord generationWord = new GenerationWord();
        public bool STOP = false;
        List<string> words = new List<string>();
        public void CreateDictionary()
        {
            string[] array = File.ReadAllLines("dict.txt");
            words = array.ToList();
        }
        async public void Generation(UiElements uiElements, List<Label> listLabel, List<Border> listBorder)
        {
            uiElements.GenButton.Visibility = Visibility.Hidden;

            if (uiElements.VisualCheckBox.IsChecked == true)
            {
                uiElements.GenStopButton.Visibility = Visibility.Visible;
            }
            await Task.Delay(100);
            List<Word> ListWord = new List<Word>();
            List<Border> listWhiteBorder = new List<Border>();
            List<Label> listWhiteLabel = new List<Label>();
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
                    SaveWordRight(ref ListWord, ref count, numberColumn, numberRow, listWhiteBorder, listWhiteLabel);
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
                    SaveWordDown(ref ListWord, ref count, numberColumn, numberRow, listWhiteBorder, listWhiteLabel);
                }
            }

            //Поиск соединённых слов
            if (ListWord.Count > 0)
            {
                //Создать в словах список с присоединёнными словами
                List<Word> listMatchWord = new List<Word>();
                List<Word> tempListWord = new List<Word>(ListWord);
                Word newWord2 = tempListWord[0];
                listMatchWord.Add(newWord2);
                tempListWord.RemoveAt(0);
                SearchMatch(ref listMatchWord, ref tempListWord, newWord2);
                ListWord = listMatchWord;
            }

            //Подбор слов по длине
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

            //Расстановка слов
            foreach (var label in listLabel)
            {
                label.Content = null;
            }
            NewGen(uiElements, ListWord, listWhiteBorder, listWhiteLabel);
        }
        async void NewGen(UiElements uiElements, List<Word> ListWord, List<Border> listWhiteBorder, List<Label> listWhiteLabel)
        {
            uiElements.WindowsText.Content = "";
            int count = ListWord.Count;
            List<string> wordsGrid = new List<string>();
            int countGen = 0;
            int maxGen = 0;
            try
            {
                maxGen = Int32.Parse(uiElements.CountGen.Text);
            }
            catch
            {
                MessageBox.Show("Вводите только цифры в количество генераций");
            }
            bool nextSideRight = true;
            //Заменить этот цикл цепочкой методов
            for (int i = 0; i < count; i++)
            {
                countGen++;
                if (countGen > maxGen)
                {
                    uiElements.WindowsText.Content = "Генерация не удалась";
                    uiElements.GenStopButton.Visibility = Visibility.Hidden;
                    uiElements.GenButton.Visibility = Visibility.Visible;
                    return;
                }
                if (STOP == true)
                {
                    STOP = false;
                    uiElements.WindowsText.Content = "Генерация остановлена";
                    uiElements.GenStopButton.Visibility = Visibility.Hidden;
                    uiElements.GenButton.Visibility = Visibility.Visible;
                    return;
                }
                bool right = ListWord[i].GetRight();
                bool down = ListWord[i].GetDown();
                Word newWord = ListWord[i];
                bool firstWordError = false;
                bool secondWordError = false;
                if (nextSideRight == false)
                {
                    if (down == true)
                    {
                        Label YellowLabel = ListWord[i].GetFirstLabel();
                        YellowLabel.Background = Brushes.Yellow;
                        MessageBox.Show("По вертикали");
                        bool right2 = false;
                        firstWordError = generationWord.InsertWord(right2, ListWord[i].GetFirstLabel(), listWhiteBorder, listWhiteLabel, ListWord[i].GetDownListWords(), ref wordsGrid, ref newWord);
                        YellowLabel.Background = Brushes.White;
                        nextSideRight = true;
                    }
                    if (right == true && firstWordError == false)
                    {
                        Label YellowLabel = ListWord[i].GetFirstLabel();
                        YellowLabel.Background = Brushes.Yellow;
                        MessageBox.Show("По горизонтали");
                        bool right2 = true;
                        secondWordError = generationWord.InsertWord(right2, ListWord[i].GetFirstLabel(), listWhiteBorder, listWhiteLabel, ListWord[i].GetRightListWords(), ref wordsGrid, ref newWord);
                        YellowLabel.Background = Brushes.White;
                        nextSideRight = false;
                    }
                }
                else
                {
                    if (right == true)
                    {
                        Label YellowLabel = ListWord[i].GetFirstLabel();
                        YellowLabel.Background = Brushes.Yellow;
                        MessageBox.Show("По горизонтали");
                        bool right2 = true;
                        secondWordError = generationWord.InsertWord(right2, ListWord[i].GetFirstLabel(), listWhiteBorder, listWhiteLabel, ListWord[i].GetRightListWords(), ref wordsGrid, ref newWord);
                        YellowLabel.Background = Brushes.White;
                        nextSideRight = false;
                    }
                    if (down == true && firstWordError == false)
                    {
                        Label YellowLabel = ListWord[i].GetFirstLabel();
                        YellowLabel.Background = Brushes.Yellow;
                        MessageBox.Show("По вертикали");
                        bool right2 = false;
                        firstWordError = generationWord.InsertWord(right2, ListWord[i].GetFirstLabel(), listWhiteBorder, listWhiteLabel, ListWord[i].GetDownListWords(), ref wordsGrid, ref newWord);
                        YellowLabel.Background = Brushes.White;
                        nextSideRight = true;
                    }
                }

                ListWord.RemoveAt(i);
                ListWord.Insert(i, newWord);
                if (firstWordError || secondWordError)
                {
                    // Удлалить одно пересекающееся слово
                    if (firstWordError == true)
                    {
                        wordsGrid.RemoveAt(wordsGrid.Count - 1);
                        ListWord[i].ClearLabelDown();
                        //ListWord[i].ClearLabelRight();
                        i--;
                        MessageBox.Show("Ошибка по вертикали");
                    }
                    if (secondWordError == true)
                    {
                        wordsGrid.RemoveAt(wordsGrid.Count - 1);
                        ListWord[i].ClearLabelRight();
                        //ListWord[i].ClearLabelDown();
                        i--;
                        MessageBox.Show("Ошибка по горизонтали");

                    }
                }
                else if (uiElements.VisualCheckBox.IsChecked == true)
                {
                    await Task.Delay(1);
                }
                uiElements.CountGoodGen.Content = countGen;
            }
            foreach (var item in wordsGrid)
            {
                uiElements.WindowsText.Content += item + "\n";
            }
            uiElements.CountGoodGen.Content = countGen;
            uiElements.GenStopButton.Visibility = Visibility.Hidden;
            uiElements.GenButton.Visibility = Visibility.Visible;
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
                                }
                            }
                        }
                    }
                }
            }
        }
        void SaveWordRight(ref List<Word> listWord, ref int count, int firstNumColumn, int firstNumRow, List<Border> listWhiteBorder, List<Label> listWhiteLabel)
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
        void SaveWordDown(ref List<Word> listWord, ref int count, int firstNumColumn, int firstNumRow, List<Border> listWhiteBorder, List<Label> listWhiteLabel)
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
    }
}
