using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StepBack
{
    public static async Task Get(Word newWord)
    {
        Random rnd = new Random();
        for (int i = 0; i < newWord.connectionWords.Count; i++)
        {
            Word temp = newWord.connectionWords[i];
            int randomIndex = rnd.Next(0, newWord.connectionWords.Count - 1);
            newWord.connectionWords[i] = newWord.connectionWords[randomIndex];
            newWord.connectionWords[randomIndex] = temp;
        }

        foreach (var word in newWord.connectionWords)
        {
            if (word.full)
            {
                ClearConnectionLabel.Get(word);

                if (!InsertWordGrid.Get(newWord))
                {
                    if (Global.visualization.IsChecked == true)
                    {
                        TestWordStart.Get(newWord, Brushes.Green);
                        await Task.Delay(Global.taskDelay);
                        TestWordEnd.Get(newWord);
                    }

                    break;
                }
            }
        }

        Global.index = 0;
    }
}