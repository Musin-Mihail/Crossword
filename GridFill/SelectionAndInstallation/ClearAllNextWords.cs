using System.Threading.Tasks;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearAllNextWords
{
    public static async Task Get()
    {
        for (int i = Global.index + 1; i < Global.listWordsGrid.Count; i++)
        {
            if (!Global.listWordsGrid[i].full)
            {
                await ClearConnectionLabel.Get(Global.listWordsGrid[i]);
            }
        }
    }
}