using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchWord
{
    public static string Get(Word word)
    {
        if (word.ListLabel.Count > 1)
        {
            var gridWord = "";
            foreach (var label in word.ListLabel)
            {
                if (label.Content != null)
                {
                    gridWord += label.Content;
                }
                else
                {
                    gridWord += '*';
                }
            }

            ListWordsRandomization.Get(word);
            foreach (var dictionary in word.FullDictionaries)
            {
                if (!CheckDictionary.Get(dictionary.Name))
                {
                    continue;
                }

                foreach (var dictionaryWord in dictionary.Words)
                {
                    if (dictionaryWord.Answers.Length == gridWord.Length)
                    {
                        for (var i = 0; i < gridWord.Length; i++)
                        {
                            if (gridWord[i] != '*')
                            {
                                if (dictionaryWord.Answers[i] != gridWord[i])
                                {
                                    break;
                                }
                            }

                            if (i == dictionaryWord.Answers.Length - 1)
                            {
                                if (App.GameState.AllInsertedWords.Contains(dictionaryWord.Answers) == false)
                                {
                                    App.GameState.AllInsertedWords.Add(dictionaryWord.Answers);
                                    var answers = dictionaryWord.Answers;
                                    SearchDictionaryEntryAdd.Get(answers, word);
                                    return answers;
                                }
                            }
                        }
                    }
                }
            }
        }

        return "";
    }
}