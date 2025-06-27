using System;
using System.IO;
using System.Windows;

namespace Crossword.SaveLoad;

public class Save
{
    public static void Get(CrosswordState gameState)
    {
        var saveFile = "";
        foreach (var cell in gameState.ListEmptyCellStruct)
        {
            saveFile += cell.X + ";" + cell.Y + "\n";
        }

        var name = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
        File.WriteAllText(@"SaveGrid\" + name + ".grid", saveFile);
        MessageBox.Show("Сетка сохранена");
    }
}