using System.Drawing;
using System.Drawing.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Screenshot;

public class CreateFillGrid
{
    public static void Get(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, CrosswordState gameState)
    {
        var blackBrush = new SolidBrush(Color.Black);
        var blackPen = new Pen(Color.Black, 1);
        var font = new Font("Arial", 7);
        graphics.Clear(Color.White);
        foreach (var cell in gameState.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Black)
            {
                if (cell.X >= topMaxX && cell.X <= downMaxX)
                {
                    if (cell.Y >= leftMaxY && cell.Y <= rightMaxY)
                    {
                        graphics.FillRectangle(blackBrush, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                        graphics.DrawRectangle(blackPen, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                    }
                }
            }
            else
            {
                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                graphics.DrawRectangle(blackPen, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawString(cell.Label.Content.ToString(), font, blackBrush, ((cell.X - topMaxX) * sizeCell) + 19, ((cell.Y - leftMaxY) * sizeCell) + 21, format);
            }
        }

        img.Save("FillGrid.png", ImageFormat.Png);
    }
}