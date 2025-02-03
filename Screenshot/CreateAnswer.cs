using System.Collections.Generic;
using System.IO;

namespace Crossword.Screenshot;

public class CreateAnswer
{
    public static void Get(List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        string answerString = "По горизонтали: ";
        for (int i = 0; i < listDefinitionRight.Count; i++)
        {
            List<string> newListWord = new List<string>(listDefinitionRight[i].Split(';'));
            string word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        answerString += "\nПо вертикали: ";
        for (int i = 0; i < listDefinitionDown.Count; i++)
        {
            List<string> newListWord = new List<string>(listDefinitionDown[i].Split(';'));
            string word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        File.WriteAllText("Answer.txt", answerString);
    }
}