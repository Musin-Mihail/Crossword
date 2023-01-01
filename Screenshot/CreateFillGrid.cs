using System.Drawing;
using System.Drawing.Imaging;
using Crossword.Objects;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Screenshot;

public class CreateFillGrid
{
    public static void Get(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell)
    {
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen blackPen = new Pen(Color.Black, 1);
        Font font = new Font("Arial", 7);
        graphics.Clear(Color.White);
        AddingWatermarks.Get(graphics);
        foreach (Cell cell in Global.listAllCellStruct)
        {
            if (cell.border.Background == Brushes.Black)
            {
                if (cell.x >= topMaxX && cell.x <= downMaxX)
                {
                    if (cell.y >= leftMaxY && cell.y <= rightMaxY)
                    {
                        graphics.FillRectangle(blackBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                        graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                    }
                }
            }
            else
            {
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawString(cell.label.Content.ToString(), font, blackBrush, ((cell.x - topMaxX) * sizeCell) + 19, ((cell.y - leftMaxY) * sizeCell) + 21, format);
            }
        }

        img.Save("FillGrid.png", ImageFormat.Png);
    }
}