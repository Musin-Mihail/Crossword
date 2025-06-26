using System;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Words;

public class ListWordsRandomization
{
    public static void Get(Word word)
    {
        try
        {
            var rnd = new Random();
            foreach (var dictionary in word.FullDictionaries)
            {
                for (var i = 0; i < dictionary.Words.Count; i++)
                {
                    var temp = dictionary.Words[i];
                    var randomIndex = rnd.Next(0, dictionary.Words.Count - 1);
                    dictionary.Words[i] = dictionary.Words[randomIndex];
                    dictionary.Words[randomIndex] = temp;
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("ListWordsRandomization\n" + e);
        }
    }
}