using System;
using System.Collections.Generic;
using System.IO;

namespace Crossword.SaveLoad;

public class Save
{
    public static void Get(List<Cell> listEmptyCellStruct)
    {
        string saveFile = "";
        foreach (Cell cell in listEmptyCellStruct)
        {
            saveFile += cell.x + ";" + cell.y + "\n";
        }

        string name = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
        File.WriteAllText(@"SaveGrid\" + name + ".grid", saveFile);
    }
}