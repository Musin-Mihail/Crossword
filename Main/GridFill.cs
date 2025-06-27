using System.Windows;
using Crossword.FormationOfAQueue;
using Crossword.GridFill;

namespace Crossword.Main;

public class GridFill
{
    public static void Get(string maxSeconds, string taskDelay)
    {
        if (App.GameState.ListEmptyCellStruct.Count > 0)
        {
            FormationQueue.Get();
            try
            {
                App.GameState.MaxSeconds = int.Parse(maxSeconds);
                App.GameState.TaskDelay = int.Parse(taskDelay);
            }
            catch
            {
                MessageBox.Show("ОШИБКА. Водите только цифры");
                App.GameState.Stop = true;
                App.GameState.IsGenerating = false;
            }

            if (!App.GameState.Stop)
                SelectionAndInstallationOfWords.Get();
        }
    }
}