using System.Threading.Tasks;
using System.Windows;
using Crossword.FormationOfAQueue;
using Crossword.GridFill;
using Crossword.PlayingField;

namespace Crossword.Main;

public class GridFillMain
{
    public static async void Get(string maxSeconds, string taskDelay)
    {
        await Task.Delay(100);
        SearchForEmptyCells.Get();
        if (Global.listEmptyCellStruct.Count > 0)
        {
            FormationQueue.Get();
            try
            {
                Global.maxSeconds = int.Parse(maxSeconds);
                Global.taskDelay = int.Parse(taskDelay);
            }
            catch
            {
                MessageBox.Show("ОШИБКА. Водите только цифры");
            }

            await SelectionAndInstallationOfWords.Get();
        }
    }
}