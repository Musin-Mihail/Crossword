using System;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class FormationQueue
{
    public static void Get()
    {
        Global.listWordsGrid.Clear();
        SearchForTheBeginningAndLengthOfAllWords.Get();
        SearchForConnectedWords.Get();
        Random rnd = new Random();
        for (int i = 0; i < Global.listWordsGrid.Count; i++)
        {
            Word temp = Global.listWordsGrid[i];
            int randomIndex = rnd.Next(0, Global.listWordsGrid.Count - 1);
            Global.listWordsGrid[i] = Global.listWordsGrid[randomIndex];
            Global.listWordsGrid[randomIndex] = temp;
        }
    }
}