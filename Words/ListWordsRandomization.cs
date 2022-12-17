using System;

namespace Crossword.Words;

public class ListWordsRandomization
{
    public static void Get(Word word)
    {
        Random rnd = new Random();
        for (int i = 0; i < word.listTempWords.Count; i++)
        {
            string temp = word.listTempWords[i];
            int randomIndex = rnd.Next(0, word.listTempWords.Count - 1);
            word.listTempWords[i] = word.listTempWords[randomIndex];
            word.listTempWords[randomIndex] = temp;
        }
    }
}