using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StepBack
{
    public static async Task Get(Word newWord)
    {
        foreach (var word in newWord.connectionWords)
        {
            await ClearConnectionLabel.Get(word);

            int newindex = Global.listWordsGrid.IndexOf(word);
            if (newindex < Global.index)
            {
                Global.index = newindex;
            }
        }
        await ClearAllNextWords.Get();
    }
}