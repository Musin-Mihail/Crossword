using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace Crossword
{
    internal class SaveLoad
    {
        public void Save(List<Cell> listEmptyCellStruct)
        {
            string saveFile = "";
            foreach (Cell cell in listEmptyCellStruct)
            {
                saveFile += cell.x + ";" + cell.y + "\n";
            }
            string name = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
            File.WriteAllText(@"SaveGrid\" + name + ".grid", saveFile);
        }
        public void Load(List<Cell> listAllCellStruct, string[] listEmptyCellStruct)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
                cell.border.Background = Brushes.Black;
            }
            foreach (var item in listEmptyCellStruct)
            {
                List<string> strings = new List<string>(item.Split(';'));
                int x = Int32.Parse(strings[0]);
                int y = Int32.Parse(strings[1]);
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.x == x)
                    {
                        if (cell.y == y)
                        {
                            cell.border.Background = Brushes.Transparent;
                        }
                    }
                }
            }
        }
    }
}