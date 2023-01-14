﻿using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Generation
{
    public static async Task Get()
    {
        StartGeneration.Get();
        ClearAllCell.Get();
        DateTime date = DateTime.Now;
        Global.index = 0;
        int maxIndex = 0;
        while (Global.index < Global.listWordsGrid.Count)
        {
            var time = DateTime.Now - date;
            if (time.TotalSeconds > Global.maxSeconds)
            {
                Global.index = 0;
                maxIndex = 0;
                ClearAllCell.Get();
                date = DateTime.Now;
                continue;
            }

            if (Global.index > maxIndex)
            {
                date = DateTime.Now;
                maxIndex = Global.index;
                Global.windowsText.Content = "Подобрано " + Global.index + " из " + Global.listWordsGrid.Count;
                await Task.Delay(1);
            }

            if (Global.stop)
            {
                Global.stop = false;
                return;
            }

            Word newWord = Global.listWordsGrid[Global.index];
            if (newWord.full)
            {
                Global.index++;
                continue;
            }

            if (!InsertWordGrid.Get(newWord))
            {
                if (Global.visualization.IsChecked == true)
                {
                    TestWordStart.Get(newWord, Brushes.Green);
                    await Task.Delay(Global.taskDelay);
                    TestWordEnd.Get(newWord);
                }

                newWord.goodInsert++;
                if (newWord.goodInsert > Global.maxError)
                {
                    if (Global.visualization.IsChecked == true)
                    {
                        TestWordStart.Get(newWord, Brushes.Yellow);
                        await Task.Delay(Global.taskDelay);
                        TestWordEnd.Get(newWord);
                    }

                    newWord.goodInsert = 0;
                    TestWordEnd.Get(newWord);
                    await StepBack.Get(newWord);
                    continue;
                }

                Global.index++;
                continue;
            }

            if (Global.visualization.IsChecked == true)
            {
                TestWordStart.Get(newWord, Brushes.Red);
                await Task.Delay(Global.taskDelay);
                TestWordEnd.Get(newWord);
            }

            newWord.error++;
            if (newWord.error > Global.maxError)
            {
                newWord.error = 0;
                if (Global.index - 2 >= 0)
                {
                    newWord = Global.listWordsGrid[Global.index - 2];
                }
                else
                {
                    newWord = Global.listWordsGrid[0];
                }
            }

            await StepBack.Get(newWord);
        }

        if (Global.index >= Global.listWordsGrid.Count)
        {
            Success.Get();
        }

        StopGeneration.Get();
    }
}