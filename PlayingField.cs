using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Crossword
{
    public class PlayingField
    {
        int cellSize = 30;
        List<Cell> listAllCellStruct = new List<Cell>();

        public List<Cell> CreateUiGrid(Grid theGrid, MouseEventHandler moveChangeColor, MouseButtonEventHandler clickChangeColor, int numberOfCellsHorizontally, int numberOfCellsVertically)
        {
            theGrid.Children.Clear();
            for (int y = 0 + 1; y < numberOfCellsVertically + 1; y++)
            {
                for (int x = 0 + 1; x < numberOfCellsHorizontally + 1; x++)
                {
                    Cell cell = new Cell();
                    Border? border = CreateBorder(x, y, moveChangeColor, clickChangeColor);
                    theGrid.Children.Add(border);
                    Label label = CreateLabel();
                    label.FontSize = 20;
                    border.Child = label;
                    label.Margin = new Thickness(0, -4, 0, 0);
                    cell.AddBorderLabelXY(border, label, x, y);
                    listAllCellStruct.Add(cell);
                }
            }

            for (int y = 0; y < numberOfCellsVertically + 1; y++)
            {
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(0.5);
                border.Margin = new Thickness(0 * cellSize, y * cellSize, 0, 0);
                border.Width = cellSize;
                border.Height = cellSize;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Top;
                theGrid.Children.Add(border);
                Label label = CreateLabel();
                label.FontSize = 16;
                border.Child = label;
                label.Margin = new Thickness(0, -4, 0, 0);
                label.Content = y;
            }

            for (int x = 1; x < numberOfCellsHorizontally + 1; x++)
            {
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(0.5);
                border.Margin = new Thickness(x * cellSize, 0 * cellSize, 0, 0);
                border.Width = cellSize;
                border.Height = cellSize;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Top;
                theGrid.Children.Add(border);
                Label label = CreateLabel();
                label.FontSize = 16;
                border.Child = label;
                label.Margin = new Thickness(0, -4, 0, 0);
                label.Content = x;
            }
            
            Border border1 = new Border();
            border1.BorderBrush = Brushes.Black;
            border1.BorderThickness = new Thickness(0.5);
            border1.Width = cellSize;
            border1.Height = cellSize;
            border1.HorizontalAlignment = HorizontalAlignment.Left;
            border1.VerticalAlignment = VerticalAlignment.Top;
            theGrid.Children.Add(border1);

            return listAllCellStruct;
        }

        private Border? CreateBorder(int x, int y, MouseEventHandler moveChangeColor, MouseButtonEventHandler clickChangeColor)
        {
            Border? myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.MouseEnter += new MouseEventHandler(moveChangeColor);
            myBorder.MouseDown += new MouseButtonEventHandler(clickChangeColor);
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