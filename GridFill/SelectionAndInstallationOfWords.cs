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
    public static async Task Get(int maxCountGen, int maxCountWord, Label windowsText, CheckBox visualization)
    {
        for (int i = 0; i < maxCountGen; i++)
        {
            windowsText.Content = "Генерация - " + i;
            await Task.Delay(50);
            int maxError = 0;
            Global.allInsertedWords.Clear();
            foreach (Word word in Global.listWordsGrid)
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
            while (index < Global.listWordsGrid.Count)
            {
                if (Global.stop)
                {
                    windowsText.Content += "СТОП";
                    Global.stop = false;
                    return;
                }


                bool error = false;
                Word newWord = Global.listWordsGrid[index];
                if (newWord.full == false)
                {
                    error = InsertWordGrid.Get(newWord);
                }

                if (error == false)
                {
                    index++;
                    continue;
                }

                newWord.error++;
                if (newWord.error > maxCountWord)
                {
                    break;
                }

                if (newWord.error > maxError)
                {
                    maxError = newWord.error;
                    if (newWord.error % 10 == 0)
                    {
                        windowsText.Content = "Генерация - " + i + " Ошибок - " + maxError;
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
                        if (visualization.IsChecked == true)
                        {
                            TestWordStartGreen.Get(templist[t]);
                            await Task.Delay(1);
                            TestWordEnd.Get(templist[t]);
                        }

                        int newindex = Global.listWordsGrid.IndexOf(templist[t]);
                        if (newindex < index)
                        {
                            index = newindex;
                        }

                        ClearLabel.Get(templist[t]);

                        error = InsertWordGrid.Get(newWord);
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
                            if (visualization.IsChecked == true)
                            {
                                TestWordStartGreen.Get(templist[t]);
                                await Task.Delay(1);
                                TestWordEnd.Get(templist[t]);
                            }

                            int newindex = Global.listWordsGrid.IndexOf(templist[t]);
                            if (newindex < index)
                            {
                                index = newindex;
                            }

                            Reset.Get(templist[t]);
                            error = InsertWordGrid.Get(newWord);
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

                if (visualization.IsChecked == true)
                {
                    TestWordEnd.Get(newWord);
                }

                if (index != 999 && index >= 0)
                {
                    continue;
                }

                MessageBox.Show("Критическая ошибка\nНе нашёл соединённых слов\n");
                windowsText.Content += "Не нашёл соединённых слов\n";
                return;
            }

            if (index >= Global.listWordsGrid.Count)
            {
                windowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
                windowsText.Content += "Было " + i + " попыток генерации\n";
                windowsText.Content += "Максимум " + maxError + " ошибок в слове за одну генерацию\n";
                return;
            }
        }
    }
}