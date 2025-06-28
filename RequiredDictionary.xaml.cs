using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Crossword.Objects;

namespace Crossword;

public partial class RequiredDictionary : Window
{
    private readonly List<Dictionary> _availableDictionaries;

    public RequiredDictionary(List<Dictionary> availableDictionaries)
    {
        InitializeComponent();
        _availableDictionaries = availableDictionaries;
    }

    private void ButtonBase_Create(object sender, RoutedEventArgs e)
    {
        var words = Words.Text.Split(' ').ToList();
        List<DictionaryWord> dictionaryWords = new();
        foreach (var word in words)
        {
            if (!SearchMatch(word, dictionaryWords))
            {
                MessageBox.Show(word + ". Нет совпадений\nСловарь не сформирован");
                return;
            }
        }

        SaveFile(dictionaryWords);
        MessageBox.Show("Словарь сформирован\n!ОБЯЗАТЕЛЬНЫЕ.txt");
    }

    private bool SearchMatch(string word, List<DictionaryWord> dictionaryWords)
    {
        foreach (var dictionary in _availableDictionaries)
        {
            foreach (var dictionaryWord in dictionary.Words)
            {
                if (dictionaryWord.Answers.ToLower() == word.ToLower())
                {
                    dictionaryWords.Add(dictionaryWord);
                    return true;
                }
            }
        }

        return false;
    }

    private void SaveFile(List<DictionaryWord> dictionaryWords)
    {
        var newDictionary = new List<string>();
        foreach (var dictionaryWord in dictionaryWords)
        {
            var line = dictionaryWord.Answers;
            foreach (var definition in dictionaryWord.Definitions)
            {
                line += ";" + definition;
            }

            newDictionary.Add(line);
        }

        const string path = "Dictionaries/!ОБЯЗАТЕЛЬНЫЕ.txt";
        File.WriteAllLines(path, newDictionary);
    }
}