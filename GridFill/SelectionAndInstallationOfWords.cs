using System;
using System.Windows;
using Crossword.GridFill.SelectionAndInstallation;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async void Get()
    {
        try
        {
            App.GameState.AllInsertedWords.Clear();
            App.GameState.Difficulty = "Сложность - " + DifficultyLevel.Get();
            await Generation.Get();
        }
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            App.GameState.IsGenerating = false;
        }
    }
}