using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Crossword.GridFill;
using Crossword.GridFill.SelectionAndInstallation;
using Crossword.Objects;

namespace Crossword.Words;

public class CreateWordDictionary
{
    public static void Get(Word word, GenerationParameters genParams)
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
            TestWordStart.Get(word, Brushes.Red);
            MessageBox.Show("Для этого места нет подходящих слов.");
            TestWordEnd.Get(word);
            StopGeneration.Get(genParams.GridGeneration, genParams.GenStartButton, genParams.GenStopButton);
            App.GameState.Stop = true;
        }
    }
}