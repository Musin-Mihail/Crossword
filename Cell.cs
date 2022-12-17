using System.Windows.Controls;

namespace Crossword
{
    public class Cell
    {
        public Border border = new();
        public Label label = new();
        public int x;
        public int y;

        public void AddBorderLabelXY(Border border, Label label, int x, int y)
        {
            this.border = border;
            this.label = label;
            this.x = x;
            this.y = y;
        }
    }
}