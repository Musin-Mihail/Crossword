using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crossword.Objects;

namespace Crossword.PlayingField;

public static class CreateDictionary
{
    public static Dictionary Get(string path)
    {
        string[] listWordsString = File.ReadAllLines(path);
        Dictionary dictionary = new Dictionary();
        foreach (var word in listWordsString)
        {
            List<string> answerAndDefinition = word.Split(';').ToList();
            DictionaryWord dictionaryWord = new DictionaryWord();
            dictionaryWord.answers = answerAndDefinition[0];
            for (int i = 1; i < answerAndDefinition.Count; i++)
            {
                dictionaryWord.definitions.Add(answerAndDefinition[i]);
            }

            dictionary.words.Add(dictionaryWord);
        }

        return dictionary;
    }
}