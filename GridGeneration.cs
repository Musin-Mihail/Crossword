using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Linq;

namespace Crossword
{
    internal class GridGeneration
    {
        public bool STOP = false;
        List<string> words = new List<string>();

        List<Label> listLabel = new List<Label>();
        List<Border> listBorder = new List<Border>();
        UiElements uiElements = new UiElements();

        List<Word> listWord = new List<Word>();
        List<Border> listEmptyBorder = new List<Border>();
        List<Label> listEmptyLabel = new List<Label>();

        public void CreateDictionary()
        {
            string[] array = File.ReadAllLines("dict.txt");
            words = array.ToList();
        }
        public void AddBorderLabelUI(List<Border> listBorder, List<Label> listLabel, UiElements uiElements)
        {
            this.listLabel = listLabel;
            this.listBorder = listBorder;
            this.uiElements = uiElements;
        }
        public void Generation()
        {
            uiElements.GenButton.Visibility = Visibility.Hidden;
            uiElements.GenStopButton.Visibility = Visibility.Visible;
            listWord.Clear();
            listEmptyBorder.Clear();
            listEmptyLabel.Clear();

            SearchForEmptyCells();

            SearchForTheBeginningAndLengthOfAllWords();

            SearchForConnectedWords();

            SearchForWordsByLength();

            SelectionAndInstallationOfWords();

            DisplayingWordsOnTheScreen();

            uiElements.GenStopButton.Visibility = Visibility.Hidden;
            uiElements.GenButton.Visibility = Visibility.Visible;
        }
        void DisplayingWordsOnTheScreen()
        {
            uiElements.WindowsText.Content = "";
            uiElements.WindowsText.Content += "По горизонтали\n";
            string newText = "";
            // Есть пустые слова. Нужно найти как они добавляються
            foreach (var word in listWord)
            {
                var test = word.GetRightLabel();
                if (test.Count > 1)
                {
                    foreach (var label in test)
                    {
                        if (label.Content != null)
                        {
                            newText = label.Content.ToString();
                            if (newText.Length == 1)
                            {
                                uiElements.WindowsText.Content += label.Content.ToString();
                            }
                        }
                    }
                    uiElements.WindowsText.Content += "\n";
                }
            }
            uiElements.WindowsText.Content += "\nПо вертикали\n";
            foreach (var word in listWord)
            {
                var test = word.GetDownLabel();
                if (test.Count > 1)
                {
                    foreach (var label in test)
                    {
                        if (label.Content != null)
                        {
                            newText = label.Content.ToString();
                            if (newText.Length == 1)
                            {
                                uiElements.WindowsText.Content += label.Content.ToString();
                            }
                        }
                    }
                    uiElements.WindowsText.Content += "\n";
                }
            }
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
        void SelectionAndInstallationOfWords()
        {
            foreach (var label in listLabel)
            {
                label.Content = null;
            }
            NewGen2(0);
        }
        void SearchForWordsByLength()
        {
            for (int i = 0; i < listWord.Count; i++)
            {
                Word newWord = listWord[i];
                listWord.RemoveAt(i);
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
                listWord.Insert(i, newWord);
            }
            foreach (var word in listWord)
            {
                word.ListWordsRandomization();
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
        void NewGen2(int index)
        {
            if (index < listWord.Count)
            {
                Label FirsLabel = listWord[index].GetFirstLabel();
                FirsLabel.Background = Brushes.Yellow;

                Word newWord = listWord[index];
                GenerationWord generationWord = new GenerationWord();
                int error = generationWord.InsertWord(listEmptyBorder, listEmptyLabel, ref newWord);
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
                            List<Word> newListWord = newWord2.GetConnectionWordsRight();
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
                            List<Word> newListWord = newWord2.GetConnectionWordsDown();
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
        void SearchMatch(ref List<Word> listMatchWord, ref List<Word> tempListWord, Word word)
        {
            if (word.GetRight() == true)
            {
                List<Label> tempListLabel = word.GetRightLabel();
                foreach (Label label in tempListLabel)
                {
                    int listCount = tempListWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (tempListWord[i].GetDown() == true)
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
            if (word.GetDown() == true)
            {
                List<Label> tempListLabel = word.GetDownLabel();
                foreach (Label label in tempListLabel)
                {
                    int listCount = tempListWord.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (tempListWord[i].GetRight() == true)
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
