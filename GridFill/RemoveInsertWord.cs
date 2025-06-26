using Crossword.Objects;

namespace Crossword.GridFill;

public class RemoveInsertWord
{
    public static void Get(Word word)
    {
        var index = Global.AllInsertedWords.IndexOf(word.WordString);
        if (index >= 0)
        {
            SearchDictionaryEntryRemove.Get(word.WordString);
            Global.AllInsertedWords.RemoveAt(index);
        }

        word.WordString = "";
        word.Full = false;
    }
}