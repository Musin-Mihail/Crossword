using System.Threading.Tasks;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearAllNextWords
{
    public static async Task Get()
    {
        for (int j = Global.index + 1; j < Global.listWordsGrid.Count; j++)
        {
            foreach (var label in Global.listWordsGrid[j].listLabel)
            {
                if (label.Content == null)
                {
                    await RemoveInsertWord.Get(Global.listWordsGrid[j]);
                    break;
                }
            }
        }
    }
}