using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Threading;

namespace Crossword
{
    internal class GridFill
    {

        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();
        List<Word> listWordStruct = new List<Word>();
        List<List<string>> listWordsList = new List<List<string>>();
        Label WindowsText = new Label();
        public bool STOP = false;


        public void AddListAllEmptyWordsLabel(List<Cell> listAllCellStruct, List<Cell> listEmptyCellStruct, List<List<string>> listWordsList, Label WindowsText)
        {
            this.listAllCellStruct = listAllCellStruct;
            this.listEmptyCellStruct = listEmptyCellStruct;
            this.listWordsList = listWordsList;
            this.WindowsText = WindowsText;
        }
        async public Task Generation()
        {
            SearchForTheBeginningAndLengthOfAllWords();
            SearchForConnectedWords();
            DefiningTheGenerationQueue();
            SearchForWordsByLength();
            await SelectionAndInstallationOfWords();
            //DisplayingWordsOnTheScreen();
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
                List<Word> listAllHardestWord = new List<Word>();
                List<Word> tempListWord = new List<Word>();
                List<Word> allRightWord = new List<Word>();

                listAllHardestWord.Add(theHardestWord);

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
                            foreach (Word word3 in listAllHardestWord)
                            {
                                if (word2.listLabel == word3.listLabel)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            foreach (Word word3 in listMatchWord)
                            {
                                if (word2.listLabel == word3.listLabel)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (match == false)
                            {
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
                if (word.difficulty == maxDifficulty)
                {
                    if (word.listLabel.Count > maxWordCount)
                    {
                        maxWordCount = word.listLabel.Count;
                        theHardestWord = word;
                    }
                }
                else if (word.difficulty > maxDifficulty)
                {
                    maxDifficulty = word.difficulty;
                    theHardestWord = word;
                    maxWordCount = word.listLabel.Count;
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
            }
            foreach (var word in listWordStruct)
            {
                word.ListWordsRandomization();
            }
        }
        async public Task SelectionAndInstallationOfWords()
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
            }
            //int index = 0;

            WindowsText.Content = "";
            //int countWord = 0;
            //int countBack = 0;
            //int maxCountBack = 0;
            //int maxCountWord = 0;
            //try
            //{
            //    maxCountWord = Int32.Parse(CountGenWord.Text);
            //}
            //catch
            //{
            //    MessageBox.Show("Введите цифры в количество попыток для слова");
            //    return;
            //}

            //try
            //{
            //    maxCountBack = Int32.Parse(CountGen.Text);
            //}
            //catch
            //{
            //    MessageBox.Show("Введите цифры в количество попыток для всей генерации");
            //    return;
            //}
            Word newWord = listWordStruct[0];
            while (true)
            {
                await Task.Delay(200);
                if (STOP)
                {
                    WindowsText.Content += "СТОП";
                    return;
                }
                bool error = InsertWord(newWord);

                //if (countWord > maxCountWord)
                //{
                //countBack++;


                //if (countBack > maxCountBack)
                //{
                //    MessageBox.Show("Новая генерация" + countBack + " " + index + " " + countWord);
                //    for (int i = 0; i < listWordStruct.Count; i++)
                //    {
                //        Word newWord2 = listWordStruct[i];
                //        newWord2.Reset();
                //        RefreshWord(i, newWord2);
                //    }
                //    countBack = 0;
                //    index = 0;
                //    countWord = 0;
                //    continue;
                //}
                //else
                //{
                //    WindowsText.Content = "ОШИБКА ГЕНЕРАЦИИ\nОШИБКА ГЕНЕРАЦИИ\nОШИБКА ГЕНЕРАЦИИ\n";
                //    break;
                //}
                //}
                int index = listWordStruct.IndexOf(newWord);
                if (error == false)
                {
                    if (index > listWordStruct.Count - 1)
                    {
                        return;
                    }
                    else if (index == listWordStruct.Count - 1)
                    {

                        continue;
                    }
                    newWord = listWordStruct[index + 1];
                    continue;
                }
                else
                {

                    int count = listWordStruct.Count;
                    bool match = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (match == true)
                        {
                            break;
                        }
                        if (newWord.listLabel != listWordStruct[i].listLabel)
                        {
                            int indexLabel = newWord.listLabel.Count - 1;
                            for (int c = 0; c < listWordStruct[i].listLabel.Count - 1; c++)
                            {
                                listWordStruct[i].listLabel[c].Background = Brushes.Red;
                                newWord.listLabel[indexLabel].Background = Brushes.Blue;
                                //MessageBox.Show("Проверяю букву ");
                                if (listWordStruct[i].listLabel[c] == newWord.listLabel[indexLabel])
                                {
                                    foreach (var item in listWordStruct[i].listLabel)
                                    {
                                        item.Background = Brushes.Red;
                                    }
                                    listWordStruct[i].listLabel[c].Background = Brushes.Blue;
                                    //MessageBox.Show("Нашёл пересечение");
                                    if (listWordStruct[i].listLabel[c].Content != null)
                                    {
                                        //MessageBox.Show("Оно непустое");
                                        newWord = listWordStruct[i];
                                        newWord.ClearLabel();
                                        match = true;
                                        break;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("Но оно пустое");
                                        indexLabel--;
                                        c = 0;
                                    }
                                }

                            }
                        }
                    }
                    continue;
                    //Word newWord2 = listWordStruct[index];
                    //if (index > 0)
                    //{
                    //    newWord2.RefreshListLabel();
                    //    newWord2.ClearLabel();
                    //    newWord2.RestoreDictionary();
                    //    RefreshWord(index, newWord2);

                    //    listWordStruct[index - 1].ClearLabel();
                    //    index--;
                    //    continue;
                    //}
                    //else
                    //{
                    //    newWord2.ClearLabel();
                    //    continue;
                    //}
                }
            }
        }
        bool InsertWord(Word word)
        {
            bool error = false;
            Label firstLabel = word.firstLabel;
            int x = GetXCellLabel(firstLabel);
            int y = GetYCellLabel(firstLabel);

            List<string> words = word.listTempWords;
            if (words.Count == 0)
            {
                return true;
            }
            else
            {
                List<Label> newListLabel = SearchEmptyLineRight(x, y);
                if (newListLabel.Count > 0)
                {
                    error = SearchWord(newListLabel, words, word);
                    if (error == true)
                    {
                        return true;
                    }
                }
                newListLabel = SearchEmptyLineDown(x, y);
                if (newListLabel.Count > 0)
                {
                    error = SearchWord(newListLabel, words, word);
                    if (error == true)
                    {
                        return true;
                    }
                }
            }
            return false;
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
        public int GetXCellLabel(Label label)
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
        public int GetYCellLabel(Label label)
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
        bool SearchWord(List<Label> newListLabel, List<string> words, Word word)
        {
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count > 1)
                {
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
                                foreach (var item2 in newListLabel)
                                {
                                    item2.Background = Brushes.Red;
                                }
                                //MessageBox.Show("нет подходящего слов");
                                WindowsText.Content += "нет подходящего слов\n";
                                foreach (var item2 in newListLabel)
                                {
                                    item2.Background = Brushes.Transparent;
                                }
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
                    WindowsText.Content += newWord + "\n";
                    //MessageBox.Show(newWord);
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

    }
}
