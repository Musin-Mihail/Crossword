using System;

namespace Crossword.PlayingField;

public class ListRandomization
{
    public static void Get(string[] words)
    {
        Random rnd = new Random();
        for (int i = 0; i < words.Length; i++)
        {
            string temp = words[i];
            int randomIndex = rnd.Next(0, words.Length - 1);
            words[i] = words[randomIndex];
            words[randomIndex] = temp;
        }
    }
}