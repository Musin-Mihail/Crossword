using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.GridFill;

public class Generation
{
    static async public Task Get(int maxCounGen, int maxCounWord, List<Word> listWordStruct, List<Dictionary> listWordsList, Label windowsText, CheckBox visualization)
    {
        List<string> allInsertedWords = new();
        foreach (Word word in listWordStruct)
        {
            word.allInsertedWords = allInsertedWords;
        }

        SearchForWordsByLength.Get(listWordStruct, listWordsList);
        await SelectionAndInstallationOfWords.Get(allInsertedWords, maxCounGen, maxCounWord, listWordStruct, windowsText, visualization);
    }
}