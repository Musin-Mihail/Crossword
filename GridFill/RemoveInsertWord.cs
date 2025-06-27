using Crossword.Objects;

namespace Crossword.GridFill;

public class RemoveInsertWord
{
    public static void Get(Word word)
    {
        var index = App.GameState.AllInsertedWords.IndexOf(word.WordString);
        if (index >= 0)
        {
            SearchDictionaryEntryRemove.Get(word.WordString);
            App.GameState.AllInsertedWords.RemoveAt(index);
        }

        word.WordString = "";
        word.Full = false;
    }
}