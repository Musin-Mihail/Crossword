using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crossword.Objects;

namespace Crossword.Services
{
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
                var answerAndDefinition = word.Split(';').ToList();
                if (answerAndDefinition.Count == 0) continue;

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
}