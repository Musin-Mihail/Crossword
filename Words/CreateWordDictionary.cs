using System.Collections.Generic;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Words;

public class CreateWordDictionary
{
    public static void Get(Word word)
    {
        var zero = true;
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            List<DictionaryWord> newDictionaryWord = new();
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.Length == word.ListLabel.Count)
                {
                    newDictionaryWord.Add(dictionaryWord);
                }
            }

            Dictionary newDictionary = new()
            {
                Name = dictionary.Name,
                Words = new List<DictionaryWord>(newDictionaryWord)
            };
            if (newDictionary.Words.Count > 0)
            {
                zero = false;
            }

            word.FullDictionaries.Add(newDictionary);
        }

        if (zero)
        {
            MessageBox.Show("Для этого места нет подходящих слов. Генерация остановлена.");
            App.GameState.Stop = true;
        }
    }
}