using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Crossword.ViewModel;
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
        var count = 0;
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

        foreach (var word in gameState.ListWordsGrid)
        {
            if (!word.ListLabel.Any()) continue;
            var startingCellState = gameState.ListAllCellStruct.FirstOrDefault(s => s.Label == word.ListLabel[0]);
            if (startingCellState == null) continue;
            count++;
            var text = count + ";" + word.WordString;
            if (word.Right)
            {
                listDefinitionRight.Add(text);
            }
            else
            {
                listDefinitionDown.Add(text);
            }

            var drawX = (startingCellState.X - topMaxX) * sizeCell;
            var drawY = (startingCellState.Y - leftMaxY) * sizeCell;
            graphics.DrawString(count.ToString(), font, blackBrush, drawX + 1, drawY + 1);
        }

        img.Save("EmptyGrid.png", ImageFormat.Png);
    }
}