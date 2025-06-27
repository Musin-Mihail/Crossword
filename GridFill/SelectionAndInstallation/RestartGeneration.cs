using System;

namespace Crossword.GridFill.SelectionAndInstallation;

public class RestartGeneration
{
    public static void Get()
    {
        App.GameState.Index = 0;
        ClearAllCell.Get();
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            dictionary.CurrentCount = 0;
        }

        var rnd = new Random();
        for (var i = 0; i < App.GameState.ListWordsGrid.Count; i++)
        {
            var temp = App.GameState.ListWordsGrid[i];
            var randomIndex = rnd.Next(0, App.GameState.ListWordsGrid.Count - 1);
            App.GameState.ListWordsGrid[i] = App.GameState.ListWordsGrid[randomIndex];
            App.GameState.ListWordsGrid[randomIndex] = temp;
        }
    }
}