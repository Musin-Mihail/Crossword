using System;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Words;

public class ListWordsRandomization
{
    public static void Get()
    {
        try
        {
            Random rnd = new Random();
            foreach (var dictionary in Global.listDictionaries)
            {
                for (int i = 0; i < dictionary.words.Count; i++)
                {
                    DictionaryWord temp = dictionary.words[i];
                    int randomIndex = rnd.Next(0, dictionary.words.Count - 1);
                    dictionary.words[i] = dictionary.words[randomIndex];
                    dictionary.words[randomIndex] = temp;
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("ListWordsRandomization\n" + e);
        }
    }
}