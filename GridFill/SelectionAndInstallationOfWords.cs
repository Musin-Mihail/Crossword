using System;
using System.Threading.Tasks;
using System.Windows;
using Crossword.GridFill.SelectionAndInstallation;

namespace Crossword.GridFill;

public class SelectionAndInstallationOfWords
{
    public static async Task Get(int maxCountGen, int maxCountWord, int taskDelay)
    {
        try
        {
            Global.allInsertedWords.Clear();
            await Generation.Get(maxCountGen, DifficultyLevel.Get(), taskDelay, maxCountWord);
        }
        catch (Exception e)
        {
            MessageBox.Show("" + e);
            throw;
        }
    }
}