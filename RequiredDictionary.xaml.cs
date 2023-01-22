using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Crossword.Objects;

namespace Crossword;

public partial class RequiredDictionary : Window
{
    public RequiredDictionary()
    {
        InitializeComponent();
    }

    private void ButtonBase_Create(object sender, RoutedEventArgs e)
    {
        List<string> words = Words.Text.Split(' ').ToList();
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

    bool SearchMatch(string word, List<DictionaryWord> dictionaryWords)
    {
        foreach (var dictionary in Global.listDictionaries)
        {
            foreach (var dictionaryWord in dictionary.words)
            {
                if (dictionaryWord.answers.ToLower() == word.ToLower())
                {
                    dictionaryWords.Add(dictionaryWord);
                    return true;
                }
            }
        }

        return false;
    }

    void SaveFile(List<DictionaryWord> dictionaryWords)
    {
        List<string> newDictionary = new List<string>();
        foreach (var dictionaryWord in dictionaryWords)
        {
            string line = dictionaryWord.answers;
            foreach (var definition in dictionaryWord.definitions)
            {
                line += ";" + definition;
            }

            newDictionary.Add(line);
        }

        var path = "Dictionaries/!ОБЯЗАТЕЛЬНЫЕ.txt";
        File.WriteAllLines(path, newDictionary);
    }
}