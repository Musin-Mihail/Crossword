using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword;

public partial class LoadGrid : Window
{
    public string[] ListEmptyCellStruct = Array.Empty<string>();
    private string[] _allGrid = Array.Empty<string>();
    public bool Ready;

    public LoadGrid()
    {
        InitializeComponent();
        DrawGrids();
    }

    private void DrawGrids()
    {
        TheGrid.Children.Clear();
        _allGrid = Directory.GetFiles(@"SaveGrid\", "*.grid");
        var tempAllGrid = _allGrid.ToList();
        var nextGrid = 0;
        for (var x = 0; x < 1000; x++)
        {
            var moveRight = 0;
            for (var y = 0; y < 4; y++)
            {
                if (tempAllGrid.Count == 0)
                {
                    return;
                }

                ListEmptyCellStruct = File.ReadAllLines(tempAllGrid[0]);
                var border = CreateBlackBorder(nextGrid, moveRight);
                TheGrid.Children.Add(border);
                foreach (var cell in ListEmptyCellStruct)
                {
                    var strings = new List<string>(cell.Split(';'));
                    var x2 = int.Parse(strings[0]) - 1;
                    var y2 = int.Parse(strings[1]) - 1;
                    border = CreateWhiteBorder(x2, y2, nextGrid, moveRight);
                    TheGrid.Children.Add(border);
                }

                var index = Array.IndexOf(_allGrid, tempAllGrid[0]);
                var button = CreateButtonAdd(nextGrid, index, moveRight);
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
        var myBorder = new Border
        {
            Background = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(moveRight * 250, nextGrid * 155, 0, 0),
            Width = 150,
            Height = 150
        };
        return myBorder;
    }

    private Border CreateWhiteBorder(int x, int y, int nextGrid, int moveRight)
    {
        var myBorder = new Border
        {
            Background = Brushes.White,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.5),
            Margin = new Thickness(x * 5 + moveRight * 250, (y * 5) + (nextGrid * 155), 0, 0),
            Width = 5,
            Height = 5,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        return myBorder;
    }

    private Button CreateButtonAdd(int nextGrid, int index, int moveRight)
    {
        var button = new Button
        {
            Name = "b" + index,
            Content = "Загрузить",
            Margin = new Thickness(160 + moveRight * 250, 50 + (nextGrid * 155), 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        button.Click += Button_Load;

        return button;
    }

    private Button CreateButtonDelete(int nextGrid, int index, int moveRight)
    {
        var button = new Button
        {
            Name = "b" + index,
            Content = "Удалить",
            Margin = new Thickness(160 + moveRight * 250, 90 + (nextGrid * 155), 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        button.Click += Button_Delete;
        return button;
    }

    private void Button_Load(object sender, RoutedEventArgs e)
    {
        var index = int.Parse(((Button)sender).Name.Remove(0, 1));
        ListEmptyCellStruct = File.ReadAllLines(_allGrid[index]);
        Ready = true;
        DialogResult = true;
    }

    private void Button_Delete(object sender, RoutedEventArgs e)
    {
        var index = int.Parse(((Button)sender).Name.Remove(0, 1));
        File.Delete(_allGrid[index]);
        DrawGrids();
    }
}