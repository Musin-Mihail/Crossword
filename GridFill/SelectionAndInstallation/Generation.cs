using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Generation
{
    public static async Task Get(GenerationParameters genParams)
    {
        StartGeneration.Get(genParams.GridGeneration, genParams.GenStartButton, genParams.GenStopButton);
        var maxIndex = 0;
        var startDate = DateTime.Now;
        var date = DateTime.Now;
        RestartGeneration.Get();
        while (App.GameState.Index < App.GameState.ListWordsGrid.Count)
        {
            var time = DateTime.Now - date;
            if (time.TotalSeconds > App.GameState.MaxSeconds)
            {
                maxIndex = 0;
                date = DateTime.Now;
                RestartGeneration.Get();
                continue;
            }

            if (App.GameState.Index > maxIndex)
            {
                date = DateTime.Now;
                maxIndex = App.GameState.Index;
                genParams.WindowsTextTop.Content = "Подобрано " + App.GameState.Index + " из " + App.GameState.ListWordsGrid.Count;
                await Task.Delay(1);
            }

            if (App.GameState.Stop)
            {
                App.GameState.Stop = false;
                return;
            }

            var newWord = App.GameState.ListWordsGrid[App.GameState.Index];
            if (newWord.Full)
            {
                App.GameState.Index++;
                continue;
            }

            if (!InsertWordGrid.Get(newWord))
            {
                if (genParams.Visualization.IsChecked == true)
                {
                    TestWordStart.Get(newWord, Brushes.Green);
                    await Task.Delay(App.GameState.TaskDelay);
                    TestWordEnd.Get(newWord);
                }

                App.GameState.Index++;
                continue;
            }

            if (genParams.Visualization.IsChecked == true)
            {
                TestWordStart.Get(newWord, Brushes.Red);
                await Task.Delay(App.GameState.TaskDelay);
                TestWordEnd.Get(newWord);
            }

            await StepBack.Get(newWord, genParams);
        }

        if (App.GameState.Index >= App.GameState.ListWordsGrid.Count)
        {
            Success.Get(startDate, genParams.WindowsTextTop);
        }

        StopGeneration.Get(genParams.GridGeneration, genParams.GenStartButton, genParams.GenStopButton);
    }
}