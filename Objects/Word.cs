using System.Collections.Generic;
using System.Windows.Controls;

namespace Crossword.Objects;

public class Word
{
    public List<Label> listLabel = new();
    public readonly List<Label> connectionLabel = new();
    public List<Word> connectionWords = new();
    public bool full = false;
    public string wordString = "";
    public bool right = false;
    public int error = 0;
    public List<Dictionary> fullDictionaries = new();
    public List<Dictionary> workDictionaries = new();

    public void RestoreDictionaries()
    {
        workDictionaries = new List<Dictionary>();
        foreach (var dictionary in fullDictionaries)
        {
            Dictionary newDictionary = new();
            newDictionary.name = dictionary.name;
            List<DictionaryWord> newDictionaryWordList = new();
            foreach (var dictionaryWord in dictionary.words)
            {
                DictionaryWord newDictionaryWord2 = new DictionaryWord();
                newDictionaryWord2.answers = dictionaryWord.answers;
                newDictionaryWord2.definitions = new List<string>(dictionaryWord.definitions);
                newDictionary.words.Add(newDictionaryWord2);
                newDictionaryWordList.Add(newDictionaryWord2);
            }

            newDictionary.words = new List<DictionaryWord>(newDictionaryWordList);
            workDictionaries.Add(newDictionary);
        }
    }
}