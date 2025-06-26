using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crossword.PlayingField;

public class CreateBorder
{
    public static Border Get(int x, int y, MouseEventHandler moveChangeColor, MouseButtonEventHandler clickChangeColor, int cellSize)
    {
        var myBorder = new Border
        {
            Background = Brushes.Black,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.5),
            Margin = new Thickness(x * cellSize, y * cellSize, 0, 0),
            Width = cellSize,
            Height = cellSize,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        myBorder.MouseEnter += moveChangeColor;
        myBorder.MouseDown += clickChangeColor;
        return myBorder;
    }
}