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
        workDictionaries.Add(new Dictionary());
        foreach (var dictionary in fullDictionaries)
        {
            foreach (var dictionaryWord in dictionary.words)
            {
                DictionaryWord newDictionary = new DictionaryWord();
                newDictionary.answers = dictionaryWord.answers;
                newDictionary.definitions = new List<string>(dictionaryWord.definitions);
                workDictionaries[0].words.Add(newDictionary);
            }
        }
    }
}