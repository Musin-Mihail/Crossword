using System.Windows.Controls;

namespace Crossword.Objects
{
    public class Cell
    {
        public Border Border = new();
        public Label Label = new();
        public int X;
        public int Y;

        public void AddBorderLabelXy(Border border, Label label, int x, int y)
        {
            Border = border;
            Label = label;
            X = x;
            Y = y;
        }
    }
}