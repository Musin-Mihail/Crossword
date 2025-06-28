using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Crossword.ViewModel;

namespace Crossword.Screenshot;

public class MaxCoordinateSearch
{
    public static void Get(ref int topMaxX, ref int downMaxX, ref int leftMaxY, ref int rightMaxY, IEnumerable<CellViewModel> cells)
    {
        var transparentCells = cells.Where(c => c.Background == Brushes.Transparent).ToList();
        if (!transparentCells.Any())
        {
            topMaxX = 1;
            downMaxX = 0;
            return;
        }

        topMaxX = transparentCells.Min(c => c.X);
        leftMaxY = transparentCells.Min(c => c.Y);
        downMaxX = transparentCells.Max(c => c.X);
        rightMaxY = transparentCells.Max(c => c.Y);
    }
}