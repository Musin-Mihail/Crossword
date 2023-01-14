using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Words;

public class CreateWordDictionary
{
    public static void Get(Word word)
    {
        Dictionary newDictionary = new();
        foreach (var dictionary in Global.listDictionaries)
        {
            List<DictionaryWord> newDictionaryWord = new();
            foreach (var dictionaryWord in dictionary.words)
            {
                if (dictionaryWord.answers.Length == word.listLabel.Count)
                {
                    newDictionaryWord.Add(dictionaryWord);
                }
            }

            newDictionary.words = new List<DictionaryWord>(newDictionaryWord);
        }

        word.fullDictionaries.Add(newDictionary);
    }
}