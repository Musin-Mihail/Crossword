using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async Task Get(List<string> allInsertedWords, int maxCounGen, int maxCounWord, List<Word> listWordStruct, Label WindowsText, CheckBox Visualization)
    {
        for (int i = 0; i < maxCounGen; i++)
        {
            WindowsText.Content = "Генерация - " + i;
            await Task.Delay(50);
            int maxError = 0;
            allInsertedWords.Clear();
            foreach (Word word in listWordStruct)
            {
                Reset.Get(word);
                word.error = 0;
                RestoreDictionary.Get(word);
            }

            foreach (Cell cell in Global.listEmptyCellStruct)
            {
                cell.label.Content = null;
                cell.label.Background = Brushes.Transparent;
            }

            int index = 0;
            while (index < listWordStruct.Count)
            {
                if (Global.stop)
                {
                    WindowsText.Content += "СТОП";
                    Global.stop = false;
                    return;
                }


                bool error = false;
                Word newWord = listWordStruct[index];
                if (newWord.full == false)
                {
                    error = InsertWordGrid.Get(allInsertedWords, newWord);
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
                    RestoreDictionary.Get(newWord);
                }

                List<Word> templist = new List<Word>(newWord.connectionWords);
                bool GlobalError = false;
                for (int t = templist.Count - 1; t >= 0; t--)
                {
                    if (templist[t].full == true)
                    {
                        string saveWord = templist[t].wordString;
                        if (Visualization.IsChecked == true)
                        {
                            TestWordStartGreen.Get(templist[t]);
                            await Task.Delay(1);
                            TestWordEnd.Get(templist[t]);
                        }

                        int newindex = listWordStruct.IndexOf(templist[t]);
                        if (newindex < index)
                        {
                            index = newindex;
                        }

                        ClearLabel.Get(templist[t]);

                        error = InsertWordGrid.Get(allInsertedWords, newWord);
                        if (error == false)
                        {
                            Reset.Get(templist[t]);
                            GlobalError = true;
                            break;
                        }

                        if (saveWord.Length == 0)
                        {
                            MessageBox.Show("saveWord.Length == 0");
                        }

                        InsertWord.Get(templist[t], saveWord);
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
                                TestWordStartGreen.Get(templist[t]);
                                await Task.Delay(1);
                                TestWordEnd.Get(templist[t]);
                            }

                            int newindex = listWordStruct.IndexOf(templist[t]);
                            if (newindex < index)
                            {
                                index = newindex;
                            }

                            Reset.Get(templist[t]);
                            error = InsertWordGrid.Get(allInsertedWords, newWord);
                            if (error == false)
                            {
                                break;
                            }
                        }
                    }
                }

                if (GlobalError == false && index != 0)
                {
                    RestoreDictionary.Get(newWord);
                }

                if (Visualization.IsChecked == true)
                {
                    TestWordEnd.Get(newWord);
                }

                if (index != 999 && index >= 0)
                {
                    continue;
                }

                MessageBox.Show("Критическая ошибка\nНе нашёл соединённых слов\n");
                WindowsText.Content += "Не нашёл соединённых слов\n";
                return;
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
}