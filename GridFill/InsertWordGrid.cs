using Crossword.Objects;

namespace Crossword.GridFill;

public class InsertWordGrid
{
    public static bool Get(Word word)
    {
        if (word.listTempWords.Count == 0)
        {
            return true;
        }

        string answer = SearchWord.Get(word);
        if (answer == "")
        {
            return true;
        }

        for (int i = 0; i < word.listLabel.Count; i++)
        {
            word.listLabel[i].Content = answer[i];
        }

        word.full = true;
        word.wordString = answer;

        return false;
    }
}