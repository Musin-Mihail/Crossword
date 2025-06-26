using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Generation
{
    public static async Task Get()
    {
        StartGeneration.Get();
        var maxIndex = 0;
        var startDate = DateTime.Now;
        var date = DateTime.Now;
        RestartGeneration.Get();
        while (Global.index < Global.ListWordsGrid.Count)
        {
            var time = DateTime.Now - date;
            if (time.TotalSeconds > Global.maxSeconds)
            {
                maxIndex = 0;
                date = DateTime.Now;
                RestartGeneration.Get();
                continue;
            }

            if (Global.index > maxIndex)
            {
                date = DateTime.Now;
                maxIndex = Global.index;
                Global.windowsText.Content = "Подобрано " + Global.index + " из " + Global.ListWordsGrid.Count;
                await Task.Delay(1);
            }

            if (Global.stop)
            {
                Global.stop = false;
                return;
            }

            var newWord = Global.ListWordsGrid[Global.index];
            if (newWord.Full)
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

                Global.index++;
                continue;
            }

            if (Global.visualization.IsChecked == true)
            {
                TestWordStart.Get(newWord, Brushes.Red);
                await Task.Delay(Global.taskDelay);
                TestWordEnd.Get(newWord);
            }

            await StepBack.Get(newWord);
        }

        if (Global.index >= Global.ListWordsGrid.Count)
        {
            Success.Get(startDate);
        }

        StopGeneration.Get();
    }
}