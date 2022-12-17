using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword
{
    public partial class LoadGrid : Window
    {
        public string[] listEmptyCellStruct = Array.Empty<string>();
        string[] allGrid = Array.Empty<string>();
        public bool ready = false;

        public LoadGrid()
        {
            InitializeComponent();
            DrawGrids();
        }

        void DrawGrids()
        {
            TheGrid.Children.Clear();
            allGrid = Directory.GetFiles(@"SaveGrid\", "*.grid");
            List<string> tempAllGrid = allGrid.ToList();
            int nextGrid = 0;
            for (int x = 0; x < 1000; x++)
            {
                int moveRight = 0;
                for (int y = 0; y < 4; y++)
                {
                    if (tempAllGrid.Count == 0)
                    {
                        return;
                    }

                    listEmptyCellStruct = File.ReadAllLines(tempAllGrid[0]);
                    Border border = CreateBlackBorder(nextGrid, moveRight);
                    TheGrid.Children.Add(border);
                    foreach (string cell in listEmptyCellStruct)
                    {
                        List<string> strings = new List<string>(cell.Split(';'));
                        int x2 = Int32.Parse(strings[0]) - 1;
                        int y2 = Int32.Parse(strings[1]) - 1;
                        border = CreateWhiteBorder(x2, y2, nextGrid, moveRight);
                        TheGrid.Children.Add(border);
                    }

                    int index = Array.IndexOf(allGrid, tempAllGrid[0]);
                    Button button = CreateButtonAdd(nextGrid, index, moveRight);
                    TheGrid.Children.Add(button);
                    button = CreateButtonDelete(nextGrid, index, moveRight);
                    TheGrid.Children.Add(button);
                    tempAllGrid.RemoveAt(0);
                    moveRight++;
                }

                nextGrid++;
            }
        }

        private Border CreateBlackBorder(int nextGrid, int moveRight)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            myBorder.Margin = new Thickness(moveRight * 250, nextGrid * 155, 0, 0);
            myBorder.Width = 150;
            myBorder.Height = 150;
            return myBorder;
        }

        private Border CreateWhiteBorder(int x, int y, int nextGrid, int moveRight)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.White;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.Margin = new Thickness(x * 5 + moveRight * 250, (y * 5) + (nextGrid * 155), 0, 0);
            myBorder.Width = 5;
            myBorder.Height = 5;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            return myBorder;
        }

        private Button CreateButtonAdd(int nextGrid, int index, int moveRight)
        {
            Button button = new Button();
            button.Name = "b" + index.ToString();
            button.Content = "Загрузить";
            button.Click += new RoutedEventHandler(Button_Load);
            button.Margin = new Thickness(160 + moveRight * 250, 50 + (nextGrid * 155), 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            return button;
        }

        private Button CreateButtonDelete(int nextGrid, int index, int moveRight)
        {
            Button button = new Button();
            button.Name = "b" + index.ToString();
            button.Content = "Удалить";
            button.Click += new RoutedEventHandler(Button_Delete);
            button.Margin = new Thickness(160 + moveRight * 250, 90 + (nextGrid * 155), 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            return button;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listEmptyCellStruct = File.ReadAllLines("SaveGrid.txt");
            DialogResult = false;
        }

        private void Button_Load(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name.Remove(0, 1));
            listEmptyCellStruct = File.ReadAllLines(allGrid[index]);
            ready = true;
            DialogResult = false;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name.Remove(0, 1));
            File.Delete(allGrid[index]);
            DrawGrids();
        }
    }
}