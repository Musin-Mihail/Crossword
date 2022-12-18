using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Crossword.Screenshot;

public class CreateDefinition
{
    public static void Get(List<string> listDefinitionRight, List<string> listDefinitionDown)
        {
            string[] array = File.ReadAllLines("dict.txt");
            List<string> listWordsString = array.ToList();

            string definitionString = "По горизонтали: ";
            for (int i = 0; i < listDefinitionRight.Count; i++)
            {
                foreach (string definition in listWordsString)
                {
                    List<string> newListDefinition = new List<string>(definition.Split(';'));
                    List<string> newListWord = new List<string>(listDefinitionRight[i].Split(';'));
                    string word1 = newListWord[1];
                    string word2 = newListDefinition[0];
                    if (word1 == word2)
                    {
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(1, newListDefinition.Count - 1);
                        definitionString += newListWord[0] + "." + newListDefinition[randomIndex] + ". ";
                        break;
                    }
                }
            }

            definitionString += "\nПо вертикали: ";
            for (int i = 0; i < listDefinitionDown.Count; i++)
            {
                foreach (string definition in listWordsString)
                {
                    List<string> newListDefinition = new List<string>(definition.Split(';'));
                    List<string> newListWord = new List<string>(listDefinitionDown[i].Split(';'));
                    string word1 = newListWord[1];
                    string word2 = newListDefinition[0];
                    if (word1 == word2)
                    {
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(1, newListDefinition.Count - 1);
                        definitionString += newListWord[0] + "." + newListDefinition[randomIndex] + ". ";
                        break;
                    }
                }
            }

            File.WriteAllText("Definition.txt", definitionString);
            MessageBox.Show("Кросворд сохранён");
        }
}