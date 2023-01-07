using System.Threading.Tasks;
using System.Windows.Controls;

namespace Crossword.GridFill;

public class Generation
{
    static async public Task Get(int maxCountGen, int maxCountWord, Label windowsText, CheckBox visualization)
    {
        Global.allInsertedWords.Clear();
        SearchForWordsByLength.Get();
        await SelectionAndInstallationOfWords.Get(maxCountGen, maxCountWord, windowsText, visualization);
    }
}