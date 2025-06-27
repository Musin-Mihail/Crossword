using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StepBack
{
    public static async Task Get(Word newWord, GenerationParameters genParams)
    {
        var rnd = new Random();
        for (var i = 0; i < newWord.ConnectionWords.Count; i++)
        {
            var temp = newWord.ConnectionWords[i];
            var randomIndex = rnd.Next(0, newWord.ConnectionWords.Count - 1);
            newWord.ConnectionWords[i] = newWord.ConnectionWords[randomIndex];
            newWord.ConnectionWords[randomIndex] = temp;
        }

        foreach (var word in newWord.ConnectionWords)
        {
            if (word is { Full: true, Fix: false })
            {
                ClearConnectionLabel.Get(word);

                if (!InsertWordGrid.Get(newWord))
                {
                    if (genParams.Visualization.IsChecked == true)
                    {
                        TestWordStart.Get(newWord, Brushes.Green);
                        await Task.Delay(App.GameState.TaskDelay);
                        TestWordEnd.Get(newWord);
                    }

                    break;
                }
            }
        }

        App.GameState.Index = 0;
    }
}