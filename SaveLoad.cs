using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO;

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
            File.WriteAllText("SaveGrid.txt", saveFile);
        }
        public void Load(List<Cell> listAllCellStruct)
        {
            foreach (Cell cell in listAllCellStruct)
            {
                cell.label.Content = null;
                cell.border.Background = Brushes.Black;
            }
            var test = File.ReadAllLines("SaveGrid.txt");
            foreach (var item in test)
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