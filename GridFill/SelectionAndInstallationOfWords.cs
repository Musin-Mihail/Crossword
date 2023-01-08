using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async Task Get(int maxCountGen, int maxCountWord, int taskDelay, Label windowsText, CheckBox visualization)
    {
        try
        {
            float difficultyLevel = 0;
            foreach (var word in Global.listWordsGrid)
            {
                difficultyLevel +=  (float)word.connectionLabel.Count / word.listLabel.Count;
            }

            difficultyLevel /= Global.listWordsGrid.Count;
            for (int i = 0; i < maxCountGen; i++)
            {
                windowsText.Content = "Сложность - " + difficultyLevel;
                windowsText.Content += "\nГенерация - " + i;
                await Task.Delay(1);
                int maxError = 0;
                Global.allInsertedWords.Clear();
                foreach (Word word in Global.listWordsGrid)
                {
                    ResetWord.Get(word);
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

                    Word newWord = Global.listWordsGrid[index];
                    if (newWord.full)
                    {
                        index++;
                        continue;
                    }

                    if (newWord.full == false)
                    {
                        if (!InsertWordGrid.Get(newWord))
                        {
                            if (visualization.IsChecked == true)
                            {
                                TestWordStart.Get(newWord, Brushes.Green);
                                await Task.Delay(taskDelay);
                                TestWordEnd.Get(newWord);
                            }

                            index++;
                            continue;
                        }

                        if (visualization.IsChecked == true)
                        {
                            TestWordStart.Get(newWord, Brushes.Red);
                            await Task.Delay(taskDelay);
                            TestWordEnd.Get(newWord);
                        }
                    }

                    newWord.error++;
                    if (newWord.error > maxCountWord)
                    {
                        break;
                    }
                    
                    if (newWord.error > maxError)
                    {
                        maxError = newWord.error;
                    }

                    foreach (var word in newWord.connectionWords)
                    {
                        foreach (var connectionWord in word.connectionWords)
                        {
                            int newindex = Global.listWordsGrid.IndexOf(connectionWord);
                            if (newindex < index)
                            {
                                index = newindex - 1;
                            }

                            if (connectionWord.full)
                            {
                                foreach (var label in connectionWord.listLabel)
                                {
                                    if (label.Content == null)
                                    {
                                        RemoveInsertWord.Get(connectionWord);
                                        break;
                                    }
                                }
                            }
                        }

                        int newindex2 = Global.listWordsGrid.IndexOf(word);
                        if (Global.listWordsGrid.IndexOf(word) < index)
                        {
                            index = newindex2 - 1;
                        }

                        if (index < 0)
                        {
                            index = 0;
                        }

                        for (int j = index + 1; j < Global.listWordsGrid.Count; j++)
                        {
                            if (Global.listWordsGrid[j].full)
                            {
                                foreach (var label in Global.listWordsGrid[j].listLabel)
                                {
                                    if (label.Content == null)
                                    {
                                        RemoveInsertWord.Get(Global.listWordsGrid[j]);
                                        break;
                                    }
                                }
                            }
                        }

                        foreach (var label in word.listLabel)
                        {
                            label.Content = null;
                        }

                        if (word.full)
                        {
                            RemoveInsertWord.Get(word);
                        }
                    }

                    foreach (var label in newWord.listLabel)
                    {
                        label.Content = null;
                    }
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
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            throw;
        }
    }
}