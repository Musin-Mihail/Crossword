using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Crossword
{
    struct Cell
    {
        public Border border;
        public Label label;
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
