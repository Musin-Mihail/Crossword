using System.IO;
using System.Linq;
using Crossword.Objects;

namespace Crossword.PlayingField;

public static class CreateDictionary
{
    public static Dictionary Get(string path)
    {
        var listWordsString = File.ReadAllLines(path);
        var dictionary = new Dictionary();
        foreach (var word in listWordsString)
        {
            var answerAndDefinition = word.Split(';').ToList();
            var dictionaryWord = new DictionaryWord
            {
                Answers = answerAndDefinition[0]
            };
            for (var i = 1; i < answerAndDefinition.Count; i++)
            {
                dictionaryWord.Definitions.Add(answerAndDefinition[i]);
            }

            dictionary.Words.Add(dictionaryWord);
        }

        return dictionary;
    }
}