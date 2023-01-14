using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Screenshot;

public class CreateDefinition
{
    public static void Get(List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        try
        {
            List<DictionaryWord> listWordsString = new();
            foreach (var dictionary in Global.listDictionaries)
            {
                listWordsString.AddRange(dictionary.words);
            }

            string definitionString = "По горизонтали: ";
            string testing = "";
            for (int i = 0; i < listDefinitionRight.Count; i++)
            {
                List<string> newListWord = new List<string>(listDefinitionRight[i].Split(';'));
                string word1 = newListWord[1];
                testing += "\n" + word1;
                foreach (DictionaryWord definition in listWordsString)
                {
                    string word2 = definition.answers;
                    if (word1 == word2)
                    {
                        testing += " - " + word2;
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(0, definition.definitions.Count - 1);
                        definitionString += newListWord[0] + "." + definition.definitions[randomIndex] + ". ";
                        break;
                    }
                }
            }

            MessageBox.Show(testing);

            definitionString += "\nПо вертикали: ";
            for (int i = 0; i < listDefinitionDown.Count; i++)
            {
                List<string> newListWord = new List<string>(listDefinitionDown[i].Split(';'));
                string word1 = newListWord[1];
                foreach (DictionaryWord definition in listWordsString)
                {
                    string word2 = definition.answers;
                    if (word1 == word2)
                    {
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(0, definition.definitions.Count - 1);
                        definitionString += newListWord[0] + "." + definition.definitions[randomIndex] + ". ";
                        break;
                    }
                }
            }

            File.WriteAllText("Definition.txt", definitionString);
        }
        catch (Exception e)
        {
            MessageBox.Show("CreateDefinition\n" + e);
        }
    }
}