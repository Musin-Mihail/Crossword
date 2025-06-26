using System.Collections.Generic;
using System.IO;

namespace Crossword.Screenshot;

public class CreateAnswer
{
    public static void Get(List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        var answerString = "По горизонтали: ";
        for (var i = 0; i < listDefinitionRight.Count; i++)
        {
            var newListWord = new List<string>(listDefinitionRight[i].Split(';'));
            var word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        answerString += "\nПо вертикали: ";
        for (var i = 0; i < listDefinitionDown.Count; i++)
        {
            var newListWord = new List<string>(listDefinitionDown[i].Split(';'));
            var word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        File.WriteAllText("Answer.txt", answerString);
    }
}