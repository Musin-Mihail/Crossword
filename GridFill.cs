using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword
{
    internal class GridFill
    {
        public bool STOP = false;
        public List<string> allInsertedWords = new List<string>();
        async public Task Generation(int maxCounGen, int maxCounWord, List<Word> listWordStruct, List<Cell> listEmptyCellStruct, List<List<string>> listWordsList, Label WindowsText, CheckBox Visualization)
        {
            foreach (Word word in listWordStruct)
            {
                word.allInsertedWords = allInsertedWords;
            }
            SearchForWordsByLength(listWordStruct, listWordsList);
            await SelectionAndInstallationOfWords(maxCounGen, maxCounWord, listWordStruct, listEmptyCellStruct, WindowsText, Visualization);
            //DisplayingWordsOnTheScreen();
        }
        public void SearchForWordsByLength(List<Word> listWordStruct, List<List<string>> listWordsList)
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
        async public Task SelectionAndInstallationOfWords(int maxCounGen, int maxCounWord, List<Word> listWordStruct, List<Cell> listEmptyCellStruct, Label WindowsText, CheckBox Visualization)
        {
            for (int i = 0; i < maxCounGen; i++)
            {
                WindowsText.Content = "Генерация - " + i;
                await Task.Delay(50);
                int maxError = 0;
                allInsertedWords.Clear();
                foreach (Word word in listWordStruct)
                {
                    word.Reset();
                    word.error = 0;
                    word.RestoreDictionary();
                }
                foreach (Cell cell in listEmptyCellStruct)
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

                    newWord.error++;
                    if (newWord.error > maxCounWord)
                    {
                        break;
                    }
                    if (newWord.error > maxError)
                    {
                        maxError = newWord.error;
                        if (newWord.error % 10 == 0)
                        {
                            WindowsText.Content = "Генерация - " + i + " Ошибок - " + maxError;
                            WindowsText.Content += "\nСлов в 1 слове - " + listWordStruct[0].listTempWords.Count;
                            WindowsText.Content += "\nСлов в 2 слове - " + listWordStruct[1].listTempWords.Count;
                            WindowsText.Content += "\nСлов в 3 слове - " + listWordStruct[2].listTempWords.Count;
                            await Task.Delay(1);
                        }
                    }

                    if (newWord.listTempWords.Count == 0)
                    {
                        newWord.RestoreDictionary();
                    }

                    List<Word> templist = new List<Word>(newWord.ConnectionWords);

                    bool GlobalError = false;
                    //Оптимизировать. сделать цикл чтобы менять дальше 2 слова, потом3 и такдалее.
                    for (int t = templist.Count - 1; t >= 0; t--)
                    {
                        if (templist[t].full == true)
                        {
                            string saveWord = templist[t].wordString;
                            if (Visualization.IsChecked == true)
                            {
                                TestWordStartGreen(templist[t]);
                                await Task.Delay(1);
                                TestWordEnd(templist[t]);
                            }
                            int newindex = listWordStruct.IndexOf(templist[t]);
                            if (newindex < index)
                            {
                                index = newindex;
                            }
                            templist[t].ClearLabel();

                            error = InsertWord(newWord);
                            if (error == false)
                            {
                                templist[t].Reset();
                                GlobalError = true;
                                break;
                            }
                            else
                            {
                                templist[t].InsertWord(saveWord);
                            }
                        }
                    }
                    if (GlobalError == false)
                    {
                        for (int t = templist.Count - 1; t >= 0; t--)
                        {
                            if (templist[t].full == true)
                            {
                                if (Visualization.IsChecked == true)
                                {
                                    TestWordStartGreen(templist[t]);
                                    await Task.Delay(1);
                                    TestWordEnd(templist[t]);
                                }
                                int newindex = listWordStruct.IndexOf(templist[t]);
                                if (newindex < index)
                                {
                                    index = newindex;
                                }
                                templist[t].Reset();
                                error = InsertWord(newWord);
                                if (error == false)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (GlobalError == false && index != 0)
                    {
                        newWord.RestoreDictionary();
                    }
                    if (Visualization.IsChecked == true)
                    {
                        TestWordEnd(newWord);
                    }

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
                if (index >= listWordStruct.Count)
                {
                    WindowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
                    WindowsText.Content += "Было " + i + " попыток генерации\n";
                    WindowsText.Content += "Максимум " + maxError + " ошибок в слове за одну генерацию\n";
                    return;
                }
            }
        }
        void TestWordStartRed(Word word)
        {
            foreach (var item in word.listLabel)
            {
                item.Background = Brushes.Red;
            }
        }
        void TestWordStartGreen(Word word)
        {
            foreach (var item in word.listLabel)
            {
                item.Background = Brushes.Green;
            }
        }
        void TestWordEnd(Word word)
        {
            foreach (var item in word.listLabel)
            {
                item.Background = Brushes.Transparent;
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
                        if (allInsertedWords.Contains(item) == false)
                        {
                            newWord = item;
                            allInsertedWords.Add(newWord);
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