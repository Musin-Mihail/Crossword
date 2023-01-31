using System;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class RestartGeneration
{
    public static void Get()
    {
        Global.index = 0;
        ClearAllCell.Get();
        foreach (var dictionary in Global.listDictionaries)
        {
            dictionary.currentCount = 0;
        }

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