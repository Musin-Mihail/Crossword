using Crossword.Objects;

namespace Crossword.GridFill;

public class InsertWordGrid
{
    public static bool Get(Word word)
    {
        var answer = SearchWord.Get(word);
        if (answer == "")
        {
            return true;
        }

        for (var i = 0; i < word.ListLabel.Count; i++)
        {
            word.ListLabel[i].Content = answer[i];
        }

        word.Full = true;
        word.WordString = answer;
        return false;
    }
}