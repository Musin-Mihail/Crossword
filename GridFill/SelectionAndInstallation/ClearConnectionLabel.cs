using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class ClearConnectionLabel
{
    public static async Task Get(Word word)
    {
        foreach (var label in word.listLabel)
        {
            if (!CheckConnectionLabel.Get(word, label))
            {
                label.Content = null;
            }
        }

        await RemoveInsertWord.Get(word);
    }
}