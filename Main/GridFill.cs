using System.Windows;
using Crossword.FormationOfAQueue;
using Crossword.GridFill;
using Crossword.PlayingField;

namespace Crossword.Main;

public class GridFill
{
    public static void Get(GenerationParameters genParams)
    {
        SearchForEmptyCells.Get();
        if (App.GameState.ListEmptyCellStruct.Count > 0)
        {
            FormationQueue.Get(genParams);
            try
            {
                App.GameState.MaxSeconds = int.Parse(genParams.MaxSeconds);
                App.GameState.TaskDelay = int.Parse(genParams.TaskDelay);
            }
            catch
            {
                MessageBox.Show("ОШИБКА. Водите только цифры");
            }

            SelectionAndInstallationOfWords.Get(genParams);
        }
    }
}