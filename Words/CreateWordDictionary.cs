using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Crossword.GridFill;
using Crossword.GridFill.SelectionAndInstallation;
using Crossword.Objects;

namespace Crossword.Words;

public class CreateWordDictionary
{
    public static void Get(Word word)
    {
        bool zero = true;
        foreach (var dictionary in Global.listDictionaries)
        {
            Dictionary newDictionary = new();
            newDictionary.name = dictionary.name;
            List<DictionaryWord> newDictionaryWord = new();
            foreach (var dictionaryWord in dictionary.words)
            {
                if (dictionaryWord.answers.Length == word.listLabel.Count)
                {
                    newDictionaryWord.Add(dictionaryWord);
                }
            }

            newDictionary.words = new List<DictionaryWord>(newDictionaryWord);
            if (newDictionary.words.Count > 0)
            {
                zero = false;
            }

            word.fullDictionaries.Add(newDictionary);
        }

        if (zero)
        {
            TestWordStart.Get(word, Brushes.Red);
            MessageBox.Show("Для этого места нет подходящих слов.");
            TestWordEnd.Get(word);
            StopGeneration.Get();
            Global.stop = true;
        }
    }
}