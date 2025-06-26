using System;

namespace Crossword.GridFill.SelectionAndInstallation;

public class RestartGeneration
{
    public static void Get()
    {
        Global.index = 0;
        ClearAllCell.Get();
        foreach (var dictionary in Global.ListDictionaries)
        {
            dictionary.CurrentCount = 0;
        }

        var rnd = new Random();
        for (var i = 0; i < Global.ListWordsGrid.Count; i++)
        {
            var temp = Global.ListWordsGrid[i];
            var randomIndex = rnd.Next(0, Global.ListWordsGrid.Count - 1);
            Global.ListWordsGrid[i] = Global.ListWordsGrid[randomIndex];
            Global.ListWordsGrid[randomIndex] = temp;
        }
    }
}