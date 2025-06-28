using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Crossword.Objects;
using Crossword.ViewModel;
using Point = System.Drawing.Point;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Screenshot;

public class CreateEmptyGrid
{
    public static void Get(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, List<string> listDefinitionRight, List<string> listDefinitionDown, IEnumerable<CellViewModel> cells, CrosswordState gameState)
    {
        var blackBrush = new SolidBrush(Color.Black);
        var whiteBrush = new SolidBrush(Color.White);
        var blackPen = new Pen(Color.Black, 1);
        var font = new Font("Arial", 4);
        graphics.Clear(Color.White);
        foreach (var cellVm in cells.Where(c => c.X >= topMaxX && c.X <= downMaxX && c.Y >= leftMaxY && c.Y <= rightMaxY))
        {
            if (cellVm.Background == Brushes.Transparent)
            {
                graphics.FillRectangle(whiteBrush, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
            }
            else
            {
                graphics.FillRectangle(blackBrush, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
            }
        }

        var numberedStartCells = new Dictionary<Point, int>();
        var numberCounter = 1;
        Cell GetStartingCell(Word word) => word.Cells.FirstOrDefault();
        var orderedWords = gameState.ListWordsGrid
            .Select(w => new { WordObject = w, StartCell = GetStartingCell(w) })
            .Where(x => x.StartCell != null)
            .OrderBy(x => x.StartCell.Y)
            .ThenBy(x => x.StartCell.X)
            .ThenBy(x => x.WordObject.Right ? 0 : 1)
            .Select(x => x.WordObject);

        foreach (var word in orderedWords)
        {
            var startingCellState = GetStartingCell(word);
            if (startingCellState == null) continue;
            var cellPoint = new Point(startingCellState.X, startingCellState.Y);
            if (!numberedStartCells.TryGetValue(cellPoint, out var wordNumber))
            {
                wordNumber = numberCounter++;
                numberedStartCells[cellPoint] = wordNumber;
                var drawX = (startingCellState.X - topMaxX) * sizeCell;
                var drawY = (startingCellState.Y - leftMaxY) * sizeCell;
                graphics.DrawString(wordNumber.ToString(), font, blackBrush, drawX + 1, drawY + 1);
            }

            var text = wordNumber + ";" + word.WordString;
            if (word.Right)
            {
                listDefinitionRight.Add(text);
            }
            else
            {
                listDefinitionDown.Add(text);
            }
        }

        img.Save("EmptyGrid.png", ImageFormat.Png);
    }
}