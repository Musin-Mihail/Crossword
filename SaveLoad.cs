using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword
{
    class SaveLoad
    {
        List<Border> listAllBorder = new List<Border>();
        List<Label> listAllLabel = new List<Label>();
        List<Border> listWhiteBorder = new List<Border>();
        public void Save()
        {
            listWhiteBorder.Clear();
            foreach (var border in listAllBorder)
            {
                if (border.Background == Brushes.Transparent)
                {
                    listWhiteBorder.Add(border);
                }
            }
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
            foreach (var border in listWhiteBorder)
            {
                border.Background = Brushes.Transparent;
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
