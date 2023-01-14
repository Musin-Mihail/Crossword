using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StepBack
{
    public static async Task Get(Word newWord)
    {
        foreach (var word in newWord.connectionWords)
        {
            int newindex = Global.listWordsGrid.IndexOf(word);
            if (Global.listWordsGrid.IndexOf(word) < Global.index)
            {
                Global.index = newindex;
            }

            await ClearAllNextWords.Get();
            await ClearLabel.Get(word);
        }

        await ClearLabel.Get(newWord);
    }
}