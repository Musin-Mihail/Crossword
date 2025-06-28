using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Crossword.ViewModel;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Screenshot;

public class CreateFillGrid
{
    public static void Get(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, IEnumerable<CellViewModel> cells)
    {
        var blackBrush = new SolidBrush(Color.Black);
        var blackPen = new Pen(Color.Black, 1);
        var font = new Font("Arial", 7);
        graphics.Clear(Color.White);
        foreach (var cell in cells.Where(c => c.X >= topMaxX && c.X <= downMaxX && c.Y >= leftMaxY && c.Y <= rightMaxY))
        {
            var drawX = (cell.X - topMaxX) * sizeCell;
            var drawY = (cell.Y - leftMaxY) * sizeCell;

            if (cell.Background == Brushes.Black)
            {
                graphics.FillRectangle(blackBrush, drawX, drawY, sizeCell, sizeCell);
            }
            else
            {
                var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                graphics.FillRectangle(System.Drawing.Brushes.White, drawX, drawY, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, drawX, drawY, sizeCell, sizeCell);
                graphics.DrawString(cell.Content?.ToUpper(), font, blackBrush, drawX + (sizeCell / 2), drawY + (sizeCell / 2), format);
            }
        }

        img.Save("FillGrid.png", ImageFormat.Png);
    }
}