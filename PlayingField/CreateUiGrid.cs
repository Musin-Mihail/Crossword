using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Objects;

namespace Crossword.PlayingField;

public class CreateUiGrid
{
    public static void Get(
        Grid theGrid,
        MouseEventHandler moveChangeColor,
        MouseButtonEventHandler clickChangeColor,
        int numberOfCellsHorizontally,
        int numberOfCellsVertically,
        int cellSize,
        CrosswordState gameState
    )
    {
        gameState.ListAllCellStruct.Clear();
        theGrid.Children.Clear();
        for (var y = 0 + 1; y < numberOfCellsVertically + 1; y++)
        {
            for (var x = 0 + 1; x < numberOfCellsHorizontally + 1; x++)
            {
                var cell = new Cell();
                var border = CreateBorder.Get(x, y, moveChangeColor, clickChangeColor, cellSize);
                theGrid.Children.Add(border);
                var label = CreateLabel.Get();
                label.FontSize = 20;
                border.Child = label;
                label.Margin = new Thickness(0, -1, 0, 0);
                cell.AddBorderLabelXy(border, label, x, y);
                gameState.ListAllCellStruct.Add(cell);
            }
        }

        for (var y = 0; y < numberOfCellsVertically + 1; y++)
        {
            var label = CreateLabel.Get();
            label.FontSize = 16;
            label.Margin = new Thickness(0, -1, 0, 0);
            label.Content = y;
            var border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0.5),
                Margin = new Thickness(0 * cellSize, y * cellSize, 0, 0),
                Width = cellSize,
                Height = cellSize,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Child = label
            };
            theGrid.Children.Add(border);
        }

        for (var x = 1; x < numberOfCellsHorizontally + 1; x++)
        {
            var label = CreateLabel.Get();
            label.FontSize = 16;
            label.Margin = new Thickness(0, -1, 0, 0);
            label.Content = x;
            var border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0.5),
                Margin = new Thickness(x * cellSize, 0 * cellSize, 0, 0),
                Width = cellSize,
                Height = cellSize,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Child = label
            };
            theGrid.Children.Add(border);
        }

        var border1 = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.5),
            Width = cellSize,
            Height = cellSize,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        theGrid.Children.Add(border1);
    }
}