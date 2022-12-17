using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crossword.PlayingField;

public class CreateBorder
{
    public static Border Get(int x, int y, MouseEventHandler moveChangeColor, MouseButtonEventHandler clickChangeColor, int cellSize)
    {
        Border myBorder = new Border();
        myBorder.Background = Brushes.Black;
        myBorder.BorderBrush = Brushes.Black;
        myBorder.BorderThickness = new Thickness(0.5);
        myBorder.MouseEnter += moveChangeColor;
        myBorder.MouseDown += clickChangeColor;
        myBorder.Margin = new Thickness(x * cellSize, y * cellSize, 0, 0);
        myBorder.Width = cellSize;
        myBorder.Height = cellSize;
        myBorder.HorizontalAlignment = HorizontalAlignment.Left;
        myBorder.VerticalAlignment = VerticalAlignment.Top;
        return myBorder;
    }
}