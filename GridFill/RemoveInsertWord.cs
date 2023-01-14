using System.Threading.Tasks;
using Crossword.Objects;

namespace Crossword.GridFill;

public class RemoveInsertWord
{
    public static async Task Get(Word word)
    {
        int index = Global.allInsertedWords.IndexOf(word.wordString);
        if (index >= 0)
        {
            Global.allInsertedWords.RemoveAt(index);
        }

        word.wordString = "";
        word.full = false;
        word.error = 0;
        word.goodInsert = 0;
    }
}