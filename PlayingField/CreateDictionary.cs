using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Crossword.PlayingField;

public class CreateDictionary
{
    public static List<List<string>> Get()
    {
        List<List<string>> listWordsList = new List<List<string>>();
        string[] listWordsString = File.ReadAllLines("dict.txt");
        for (int i = 0; i < 20; i++)
        {
            List<string> list = new List<string>();
            listWordsList.Add(list);
        }

        string error = "";
        foreach (string word in listWordsString)
        {
            try
            {
                string newWord = word.Split(';')[0];
                if (word.Split(';')[1].Length > 1)
                {
                    int count = newWord.Length;
                    listWordsList[count].Add(newWord);
                }
                else
                {
                    error += word + "\n";
                }
            }
            catch
            {
                error += word + "\n";
            }
        }

        if (error.Length > 2)
        {
            MessageBox.Show(error);
        }

        return listWordsList;
    }
}