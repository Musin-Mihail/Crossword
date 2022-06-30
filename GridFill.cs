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
        FormationOfAQueue formationOfAQueue = new FormationOfAQueue();
        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();
        List<Word> listWordStruct = new List<Word>();
        List<List<string>> listWordsList = new List<List<string>>();
        Label WindowsText = new Label();
        CheckBox Visualization = new CheckBox();
        public bool STOP = false;
        int maxCounGen = 0;
        int maxCounWord = 0;

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
            SearchForWordsByLength();
            await SelectionAndInstallationOfWords();
            DisplayingWordsOnTheScreen();
        }
        public void SearchForWordsByLength()
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
            for (int i = 0; i < maxCounGen; i++)
            {
                int counWord = 0;
                foreach (Cell cell in listAllCellStruct)
                {
                    cell.label.Content = null;
                    cell.label.Background = Brushes.Transparent;
                }
                foreach (Word word in listWordStruct)
                {
                    word.full = false;
                    word.wordString = "";
                }
                WindowsText.Content = "";
                int index = 0;
                while (index < listWordStruct.Count)
                {
                    if (Visualization.IsChecked == true)
                    {
                        await Task.Delay(1);
                    }
                    if (STOP)
                    {
                        WindowsText.Content += "СТОП";
                        STOP = false;
                        return;
                    }
                    bool error = false;
                    Word newWord = listWordStruct[index];
                    // НУжно перебрать всле лейблы. выйти из цикла только года лейбылы закончаться или не произоёдт ошибки подбора
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
                        counWord++;
                        if (counWord > maxCounWord)
                        {
                            break;
                        }
                        index = SearchConnect(newWord);
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
                    return;
                }
            }
        }

        bool InsertWord(Word word)
        {
            List<string> words = word.listTempWords;
            if (words.Count == 0)
            {
                return true;
            }
            else
            {
                List<Label> newListLabel = word.listLabel;
                bool error = SearchWord(newListLabel, words, word);
                if (error == true)
                {
                    return true;
                }
            }
            return false;
        }
        int SearchConnect(Word newWord)
        {
            for (int i = 0; i < newWord.ConnectionLabel.Count; i++)
            {
                foreach (Word word in listWordStruct)
                {
                    if (word != newWord)
                    {
                        foreach (Label label in word.ConnectionLabel)
                        {
                            if (newWord.ConnectionLabel[i] == label)
                            {
                                newWord.RestoreDictionary();
                                label.Content = null;
                                word.ClearLabel();
                                word.full = false;
                                return listWordStruct.IndexOf(word);
                            }
                        }
                    }
                }
            }
            return 999;
        }
        bool SearchWord(List<Label> newListLabel, List<string> words, Word word)
        {
            if (newListLabel.Count < 16)
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
                    string newWord = listWordsString[0];
                    word.DeleteWord(newWord);
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
                MessageBox.Show("Есть поле больше 15");
            }
            return false;
        }
        void DisplayingWordsOnTheScreen()
        {
            //WindowsText.Content = "";
            foreach (var word in listWordStruct)
            {
                WindowsText.Content += word.wordString + "\n";
            }
        }
    }
}