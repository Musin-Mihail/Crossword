using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Screenshot;

public class CreateDefinition
{
    public static void Get(List<string> listDefinitionRight, List<string> listDefinitionDown, CrosswordState gameState)
    {
        try
        {
            List<DictionaryWord> listWordsString = new();
            foreach (var dictionary in gameState.ListDictionaries)
            {
                listWordsString.AddRange(dictionary.Words);
            }

            var definitionString = "По горизонтали: ";
            for (var i = 0; i < listDefinitionRight.Count; i++)
            {
                var newListWord = new List<string>(listDefinitionRight[i].Split(';'));
                var word1 = newListWord[1];
                foreach (var definition in listWordsString)
                {
                    var word2 = definition.Answers;
                    if (word1 != word2) continue;
                    var rnd = new Random();
                    var randomIndex = rnd.Next(0, definition.Definitions.Count - 1);
                    definitionString += newListWord[0] + ". " + definition.Definitions[randomIndex] + ". ";
                    break;
                }
            }

            definitionString += "\nПо вертикали: ";
            for (var i = 0; i < listDefinitionDown.Count; i++)
            {
                var newListWord = new List<string>(listDefinitionDown[i].Split(';'));
                var word1 = newListWord[1];
                foreach (var definition in listWordsString)
                {
                    var word2 = definition.Answers;
                    if (word1 == word2)
                    {
                        var rnd = new Random();
                        var randomIndex = rnd.Next(0, definition.Definitions.Count - 1);
                        definitionString += newListWord[0] + ". " + definition.Definitions[randomIndex] + ". ";
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