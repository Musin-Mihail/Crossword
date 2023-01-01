using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.GridFill;

public class Generation
{
    static async public Task Get(int maxCountGen, int maxCountWord, Label windowsText, CheckBox visualization)
    {
        List<string> allInsertedWords = new();
        foreach (Word word in Global.listWordsGrid)
        {
            word.allInsertedWords = allInsertedWords;
        }

        SearchForWordsByLength.Get();
        await SelectionAndInstallationOfWords.Get(allInsertedWords, maxCountGen, maxCountWord, windowsText, visualization);
    }
}