using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class OneGeneration
{
    public static async Task Get(int i, int taskDelay, int maxCountWord, int maxError)
    {
        Global.index = 0;
        while (Global.index < Global.listWordsGrid.Count)
        {
            if (Global.stop)
            {
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
                    await Task.Delay(taskDelay);
                    TestWordEnd.Get(newWord);
                    newWord.goodInsert++;
                    if (newWord.goodInsert > maxCountWord)
                    {
                        newWord.goodInsert = 0;
                        TestWordStart.Get(newWord, Brushes.Yellow);
                        await Task.Delay(taskDelay);
                        TestWordEnd.Get(newWord);
                        await StepBack.Get(newWord);
                        //     newWord.goodInsert = 0;
                        //     if (Global.index - 1 >= 0)
                        //     {
                        //         // MessageBox.Show("if (Global.index - 2 >= 0)");
                        //         newWord = Global.listWordsGrid[Global.index - 1];
                        //     }
                        //     else
                        //     {
                        //         // MessageBox.Show("else");
                        //         newWord = Global.listWordsGrid[0];
                        //     }
                        //     await StepBack.Get(newWord);
                        continue;
                    }
                }

                Global.index++;
                continue;
            }

            if (Global.visualization.IsChecked == true)
            {
                TestWordStart.Get(newWord, Brushes.Red);
                await Task.Delay(taskDelay);
                TestWordEnd.Get(newWord);
            }

            newWord.error++;
            if (newWord.error > maxCountWord)
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

            if (newWord.error > maxError)
            {
                maxError = newWord.error;
            }

            await StepBack.Get(newWord);
        }

        if (Global.index >= Global.listWordsGrid.Count)
        {
            Success.Get(i, maxError);
            Global.stop = true;
        }
    }
}