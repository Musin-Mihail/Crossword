using System;
using System.Windows;
using Crossword.GridFill.SelectionAndInstallation;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async void Get(GenerationParameters genParams)
    {
        try
        {
            App.GameState.AllInsertedWords.Clear();
            genParams.DifficultyLevel.Content = "Сложность - " + DifficultyLevel.Get();
            await Generation.Get(genParams);
        }
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            throw;
        }
    }
}