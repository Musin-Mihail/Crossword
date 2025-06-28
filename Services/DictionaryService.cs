using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crossword.Models;
using Crossword.Services.Abstractions;

namespace Crossword.Services;

public class DictionaryService : IDictionaryService
{
    private const string DictionariesFolder = "Dictionaries";

    public IEnumerable<string> GetDictionaryPaths()
    {
        if (!Directory.Exists(DictionariesFolder))
        {
            Directory.CreateDirectory(DictionariesFolder);
            return Enumerable.Empty<string>();
        }

        return Directory.GetFiles(DictionariesFolder, "*.txt");
    }

    public Dictionary LoadDictionary(string path)
    {
        var listWordsString = File.ReadAllLines(path);
        var dictionary = new Dictionary();
        foreach (var word in listWordsString)
        {
            var answerWithDefinitions = word.Split(';').ToList();
            if (answerWithDefinitions.Count == 0) continue;

            var dictionaryWord = new DictionaryWord
            {
                Answers = answerWithDefinitions[0]
            };
            for (var i = 1; i < answerWithDefinitions.Count; i++)
            {
                dictionaryWord.Definitions.Add(answerWithDefinitions[i]);
            }

            dictionary.Words.Add(dictionaryWord);
        }

        return dictionary;
    }
}