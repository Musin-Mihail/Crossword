using System.Threading.Tasks;
using System.Windows.Media;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Generation
{
    public static async Task Get(int maxCountGen, float difficultyLevel, int taskDelay, int maxCountWord)
    {
        for (int i = 0; i < maxCountGen; i++)
        {
            if (Global.stop)
            {
                Global.stop = false;
                return;
            }

            Global.windowsText.Content = "Сложность - " + difficultyLevel;
            Global.windowsText.Content += "\nГенерация - " + i;
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

            await OneGeneration.Get(i, taskDelay, maxCountWord, maxError);
        }
    }
}