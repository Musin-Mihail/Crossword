using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crossword
{
    internal class PlayingField
    {
        int cellSize = 30;
        int numberOfCellsHorizontally = 30;
        int numberOfCellsVertically = 30;
        List<Cell> listAllCellStruct = new List<Cell>();
        public List<Cell> CreateUIGrid(Grid TheGrid, MouseEventHandler MoveChangeColor, MouseButtonEventHandler ClickChangeColor)
        {
            for (int y = 0; y < numberOfCellsHorizontally; y++)
            {
                for (int x = 0; x < numberOfCellsVertically; x++)
                {
                    Cell cell = new Cell();
                    Border border = CreateBorder(x, y, MoveChangeColor, ClickChangeColor);
                    TheGrid.Children.Add(border);
                    Label label = CreateLabel();
                    label.FontSize = 20;
                    border.Child = label;
                    label.Margin = new Thickness(0, -4, 0, 0);
                    cell.AddBorderLabelXY(border, label, x, y);
                    listAllCellStruct.Add(cell);
                }
            }
            return listAllCellStruct;
        }
        private Border CreateBorder(int x, int y, MouseEventHandler MoveChangeColor, MouseButtonEventHandler ClickChangeColor)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.MouseEnter += new MouseEventHandler(MoveChangeColor);
            myBorder.MouseDown += new MouseButtonEventHandler(ClickChangeColor);
            myBorder.Margin = new Thickness(x * cellSize, y * cellSize, 0, 0);
            myBorder.Width = cellSize;
            myBorder.Height = cellSize;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            return myBorder;
        }
        Label CreateLabel()
        {
            Label myLabel = new Label();
            myLabel.HorizontalAlignment = HorizontalAlignment.Center;
            myLabel.VerticalAlignment = VerticalAlignment.Center;
            return myLabel;
        }
        public List<Cell> SearchForEmptyCells()
        {
            List<Cell> listEmptyCellStruct = new List<Cell>();
            foreach (Cell cell in listAllCellStruct)
            {
                if (cell.border.Background == Brushes.Transparent)
                {
                    listEmptyCellStruct.Add(cell);
                }
            }
            return listEmptyCellStruct;
        }
        public List<List<string>> CreateDictionary()
        {
            List<List<string>> listWordsList = new List<List<string>>();
            string[] array = File.ReadAllLines("dict.txt");
            List<string> listWordsString = array.ToList();
            for (int i = 0; i < 20; i++)
            {
                List<string> list = new List<string>();
                listWordsList.Add(list);
            }
            string error = "";
            foreach (string word in listWordsString)
            {
                try
                {
                    string newWord = word.Split(';')[0];
                    if (word.Split(';')[1].Length > 1)
                    {
                        int count = newWord.Length;
                        listWordsList[count].Add(newWord);
                    }
                    else
                    {
                        error += word + "\n";
                    }
                }
                catch
                {
                    error += word + "\n";
                }
            }
            if (error.Length > 2)
            {
                MessageBox.Show(error);
            }
            return listWordsList;
        }
    }
}
