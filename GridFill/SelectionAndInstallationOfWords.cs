using System;
using System.Threading.Tasks;
using System.Windows;
using Crossword.GridFill.SelectionAndInstallation;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async Task Get()
    {
        try
        {
            Global.allInsertedWords.Clear();
            Global.windowsText.Content = "Сложность - " + DifficultyLevel.Get();
            await Generation.Get();
        }
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            throw;
        }
    }
}