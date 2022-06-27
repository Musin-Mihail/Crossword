using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;


namespace Crossword
{
    class SaveLoad
    {
        List<Border> listAllBorder = new List<Border>();
        List<Label> listAllLabel = new List<Label>();
        public void Save()
        {
            string saveFile = "";
            foreach (var border in listAllBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    int Column = Grid.GetColumn(border);
                    int Row = Grid.GetRow(border);
                    saveFile += Column + ";" + Row +"\n";
                }
            }
            File.WriteAllText("SaveGrid.txt", saveFile);
        }
        public void Load()
        {
            foreach (var label in listAllLabel)
            {
                label.Content = "";
            }
            foreach (var border in listAllBorder)
            {
                border.Background = Brushes.Black;
            }
            var test = File.ReadAllLines("SaveGrid.txt");
            foreach (var item in test)
            {
                List<string> strings = new List<string>(item.Split(';'));
                int Column = Int32.Parse(strings[0]);
                int Row = Int32.Parse(strings[1]);
                foreach (var item2 in listAllBorder)
                {
                    if(Grid.GetColumn(item2) == Column)
                    {
                        if(Grid.GetRow(item2) == Row)
                        {
                            item2.Background = Brushes.Transparent;
                        }
                    }
                }
            }
        }
        public void AddAllBorder(List<Border> listBorder)
        {
            listAllBorder = listBorder;
        }
        public void AddAllLabel(List<Label> listLabel)
        {
            listAllLabel = listLabel;
        }
    }
}