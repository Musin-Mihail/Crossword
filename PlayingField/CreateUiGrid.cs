using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.PlayingField;

public class CreateUiGrid
{
    public static void Get(Grid theGrid, MouseEventHandler moveChangeColor, MouseButtonEventHandler clickChangeColor, int numberOfCellsHorizontally, int numberOfCellsVertically, int cellSize)
    {
        Global.listAllCellStruct.Clear();
        theGrid.Children.Clear();
        for (int y = 0 + 1; y < numberOfCellsVertically + 1; y++)
        {
            for (int x = 0 + 1; x < numberOfCellsHorizontally + 1; x++)
            {
                Cell cell = new Cell();
                Border? border = CreateBorder.Get(x, y, moveChangeColor, clickChangeColor, cellSize);
                theGrid.Children.Add(border);
                Label label = CreateLabel.Get();
                label.FontSize = 20;
                border.Child = label;
                label.Margin = new Thickness(0, -4, 0, 0);
                cell.AddBorderLabelXy(border, label, x, y);
                Global.listAllCellStruct.Add(cell);
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
            Label label = CreateLabel.Get();
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
            Label label = CreateLabel.Get();
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
    }
}