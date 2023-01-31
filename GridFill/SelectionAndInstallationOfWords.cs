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
            Global.allInsertedWords.Clear();
            Global.difficultyLevel.Content = "Сложность - " + DifficultyLevel.Get();
            await Generation.Get();
        }
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            throw;
        }
    }
}