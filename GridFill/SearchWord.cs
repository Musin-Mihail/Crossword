using System.Collections.Generic;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchWord
{
    public static string Get(List<string> allInsertedWords, Word word)
    {
        if (word.listLabel.Count > 1)
        {
            string gridWord = "";
            foreach (var label in word.listLabel)
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
            foreach (var dictionary in word.listTempWords)
            {
                foreach (var dictionaryWord in dictionary.words)
                {
                    if (dictionaryWord.answers.Length == gridWord.Length)
                    {
                        for (int i = 0; i < gridWord.Length; i++)
                        {
                            if (gridWord[i] != '*')
                            {
                                if (dictionaryWord.answers[i] != gridWord[i])
                                {
                                    break;
                                }
                            }

                            if (i == dictionaryWord.answers.Length - 1)
                            {
                                if (allInsertedWords.Contains(dictionaryWord.answers) == false)
                                {
                                    allInsertedWords.Add(dictionaryWord.answers);
                                    DeleteWord.Get(dictionaryWord.answers, word);
                                    return dictionaryWord.answers;
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