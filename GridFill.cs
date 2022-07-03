using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using System;

namespace Crossword
{
    internal class GridFill
    {
        FormationOfAQueue formationOfAQueue = new FormationOfAQueue();
        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();
        public List<Word> listWordStruct = new List<Word>();
        List<List<string>> listWordsList = new List<List<string>>();
        Label WindowsText = new Label();
        CheckBox Visualization = new CheckBox();
        public bool STOP = false;
        int maxCounGen = 0;
        int maxCounWord = 0;
        public List<string> insertedWords = new List<string>();

        public void AddListAllEmptyWordsLabelVisual(List<Cell> listAllCellStruct, List<Cell> listEmptyCellStruct, List<List<string>> listWordsList, Label WindowsText, CheckBox Visualization)
        {
            this.listAllCellStruct = listAllCellStruct;
            this.listEmptyCellStruct = listEmptyCellStruct;
            this.listWordsList = listWordsList;
            this.WindowsText = WindowsText;
            this.Visualization = Visualization;
        }
        async public Task Generation(int maxCounGen, int maxCounWord)
        {
            this.maxCounGen = maxCounGen;
            this.maxCounWord = maxCounWord;
            listWordStruct = formationOfAQueue.FormationQueue(listAllCellStruct, listEmptyCellStruct);
            foreach (Word word in listWordStruct)
            {
                word.insertedWords = insertedWords;
            }
            SearchForWordsByLength();
            await SelectionAndInstallationOfWords();
            //DisplayingWordsOnTheScreen();

        }
        public void SearchForWordsByLength()
        {
            for (int i = 0; i < listWordStruct.Count; i++)
            {
                int letterCount = listWordStruct[i].listLabel.Count;
                listWordStruct[i].AddWords(listWordsList[letterCount]);
            }
            foreach (var word in listWordStruct)
            {
                word.ListWordsRandomization();
            }
        }
        async public Task SelectionAndInstallationOfWords()
        {
            for (int i = 0; i < maxCounGen; i++)
            {
                WindowsText.Content = "Генерация - " + i;
                await Task.Delay(50);
                int maxError = 0;
                insertedWords.Clear();
                foreach (Word word in listWordStruct)
                {
                    word.Reset();
                    word.error = 0;
                    word.RestoreDictionary();
                }
                foreach (Cell cell in listAllCellStruct)
                {
                    cell.label.Content = null;
                    cell.label.Background = Brushes.Transparent;
                }

                int index = 0;

                while (index < listWordStruct.Count)
                {

                    if (STOP)
                    {
                        WindowsText.Content += "СТОП";
                        STOP = false;
                        return;
                    }
                    bool error = false;
                    Word newWord = listWordStruct[index];
                    if (newWord.full == false)
                    {
                        error = InsertWord(newWord);
                    }
                    if (error == false)
                    {

                        index++;
                        continue;
                    }
                    else
                    {
                        if (Visualization.IsChecked == true)
                        {
                            await Task.Delay(100);
                        }
                        newWord.error++;
                        if (newWord.error > maxCounWord)
                        {
                            break;
                        }
                        if (newWord.error > maxError)
                        {
                            WindowsText.Content += newWord.error + "\n";
                            maxError = newWord.error;
                        }
                        //if (newWord.listTempWords.Count == 0)
                        //{
                            newWord.RestoreDictionary();
                        //}

                        //foreach (var item in newWord.listLabel)
                        //{
                        //    item.Background = Brushes.Green;
                        //}
                        List<Word> templist = new List<Word>(newWord.ConnectionWords);
                        //Random rnd = new Random();
                        //for (int u = 0; u < templist.Count; u++)
                        //{
                        //    Word temp = templist[u];
                        //    int randomIndex = rnd.Next(u, templist.Count - 1);
                        //    templist[u] = templist[randomIndex];
                        //    templist[randomIndex] = temp;
                        //}
                        //for (int t = 0; t < templist.Count; t++)
                        //{
                        for (int t = templist.Count - 1; t > 0; t--)
                        {
                            if (templist[t].full == true)
                            {

                                templist[t].lastWord = newWord;
                                int newindex = listWordStruct.IndexOf(templist[t]);
                                if (newindex < index)
                                {
                                    index = newindex;
                                }
                                templist[t].Reset();

                            }
                            error = InsertWord(newWord);
                            if (error == false)
                            {
                                break;
                            }
                        }

                        //foreach (var item in newWord.listLabel)
                        //{
                        //    item.Background = Brushes.Transparent;
                        //}
                        if (index != 999 && index >= 0)
                        {
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Критическая ошибка\nНе нашёл соединённых слов\n");
                            WindowsText.Content += "Не нашёл соединённых слов\n";
                            return;
                        }
                    }
                }
                if (index >= listWordStruct.Count)
                {
                    WindowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
                    WindowsText.Content += "Было " + i + " попыток генерации\n";
                    WindowsText.Content += "Максимум " + maxError + " ошибок в слове за одну генерацию\n";
                    return;
                }
            }
        }
        bool InsertWord(Word word)
        {
            if (word.listTempWords.Count == 0)
            {
                return true;
            }
            else
            {
                bool error = SearchWord(word.listLabel, word.listTempWords, word);
                if (error == true)
                {
                    return true;
                }
            }
            return false;
        }
        bool SearchWord(List<Label> newListLabel, List<string> words, Word word)
        {
            if (newListLabel.Count < 21)
            {
                if (newListLabel.Count > 1)
                {
                    List<string> listWordsString = new List<string>(words);
                    List<string> tempListString = new List<string>();
                    //Поиск слов с теми же буквами
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {
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
                                return true;
                            }
                        }
                    }
                    string newWord = "";
                    foreach (string item in listWordsString)
                    {
                        if (insertedWords.Contains(item) == false)
                        {
                            newWord = item;
                            insertedWords.Add(newWord);
                            word.DeleteWord(newWord);
                            break;
                        }
                    }
                    if (newWord.Length < 2)
                    {
                        return true;
                    }
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        newListLabel[i].Content = newWord[i];
                    }

                    word.full = true;
                    word.wordString = newWord;
                }
            }
            else
            {
                MessageBox.Show("Есть поле больше 20");
            }
            return false;
        }
        //void DisplayingWordsOnTheScreen()
        //{
        //    foreach (var word in listWordStruct)
        //    {
        //        WindowsText.Content += word.wordString + "\n";
        //    }
        //}
    }
}