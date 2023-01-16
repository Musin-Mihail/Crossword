using System.Collections.Generic;
using Crossword.Objects;

namespace Crossword.Words;

public class CreateWordDictionary
{
    public static void Get(Word word)
    {
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
            word.fullDictionaries.Add(newDictionary);
        }
    }
}