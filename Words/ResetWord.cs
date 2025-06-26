using Crossword.Objects;

namespace Crossword.Words;

public class ResetWord
{
    public static void Get(Word word)
    {
        ClearLabel.Get(word);
        word.Full = false;
        word.WordString = "";
        word.Fix = false;
    }
}