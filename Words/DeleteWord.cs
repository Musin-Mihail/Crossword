using Crossword.Objects;

namespace Crossword.Words;

public class DeleteWord
{
    public static void Get(string wordString, Word word)
    {
        for (int i = 0; i < word.listTempWords.Count; i++)
        {
            foreach (DictionaryWord dictionaryWord in word.listTempWords[i].words)
            {
                if (wordString == dictionaryWord.answers)
                {
                    word.listTempWords.RemoveAt(i);
                    if (word.listTempWords.Count == 0)
                    {
                        RestoreDictionary.Get(word);
                    }

                    return;
                }
            }
        }
    }
}