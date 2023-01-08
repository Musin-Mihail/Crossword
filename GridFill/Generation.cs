using System.Threading.Tasks;
using System.Windows.Controls;

namespace Crossword.GridFill;

public class Generation
{
    static async public Task Get(int maxCountGen, int maxCountWord, int taskDelay, Label windowsText, CheckBox visualization)
    {
        Global.allInsertedWords.Clear();
        await SelectionAndInstallationOfWords.Get(maxCountGen, maxCountWord, taskDelay, windowsText, visualization);
    }
}