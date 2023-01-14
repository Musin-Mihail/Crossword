using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearLabel
{
    public static async Task Get(Word word)
    {
        foreach (var label in word.listLabel)
        {
            label.Content = null;
        }

        await RemoveInsertWord.Get(word);
    }
}