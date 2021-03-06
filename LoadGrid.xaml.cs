using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword
{
    public partial class LoadGrid : Window
    {
        public string[] listEmptyCellStruct = new string[] { };
        string[] allGrid = new string[] { };
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
            int nextGrid = 0;
            for (int i = 0; i < allGrid.Length; i++)
            {
                listEmptyCellStruct = File.ReadAllLines(allGrid[i]);
                Border border = CreateBlackBorder(nextGrid);
                TheGrid.Children.Add(border);
                foreach (string cell in listEmptyCellStruct)
                {
                    List<string> strings = new List<string>(cell.Split(';'));
                    int x = Int32.Parse(strings[0]);
                    int y = Int32.Parse(strings[1]);
                    border = CreateWhiteBorder(x, y, nextGrid);
                    TheGrid.Children.Add(border);
                }
                Button button = CreateButtonAdd(nextGrid, i);
                TheGrid.Children.Add(button);
                button = CreateButtonDelete(nextGrid, i);
                TheGrid.Children.Add(button);
                nextGrid++;
            }
        }
        private Border CreateBlackBorder(int nextGrid)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.Black;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            myBorder.Margin = new Thickness(0, nextGrid * 155, 0, 0);
            myBorder.Width = 150;
            myBorder.Height = 150;
            return myBorder;
        }
        private Border CreateWhiteBorder(int x, int y, int nextGrid)
        {
            Border myBorder = new Border();
            myBorder.Background = Brushes.White;
            myBorder.BorderBrush = Brushes.Black;
            myBorder.BorderThickness = new Thickness(0.5);
            myBorder.Margin = new Thickness(x * 5, (y * 5) + (nextGrid * 155), 0, 0);
            myBorder.Width = 5;
            myBorder.Height = 5;
            myBorder.HorizontalAlignment = HorizontalAlignment.Left;
            myBorder.VerticalAlignment = VerticalAlignment.Top;
            return myBorder;
        }
        private Button CreateButtonAdd(int nextGrid, int index)
        {
            Button button = new Button();
            button.Name = "b" + index.ToString();
            button.Content = "Загрузить";
            button.Click += new RoutedEventHandler(Button_Load);
            button.Margin = new Thickness(160, 50 + (nextGrid * 155), 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            return button;
        }
        private Button CreateButtonDelete(int nextGrid, int index)
        {
            Button button = new Button();
            button.Name = "b" + index.ToString();
            button.Content = "Удалить";
            button.Click += new RoutedEventHandler(Button_Delete);
            button.Margin = new Thickness(160, 90 + (nextGrid * 155), 0, 0);
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
