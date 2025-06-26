using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Screenshot;

public class CreateEmptyGrid
{
    public static void Get(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        var blackBrush = new SolidBrush(Color.Black);
        var whiteBrush = new SolidBrush(Color.White);
        var blackPen = new Pen(Color.Black, 1);
        var font = new Font("Arial", 4);
        graphics.Clear(Color.White);
        var count = 0;
        graphics.FillRectangle(blackBrush, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
        graphics.DrawRectangle(blackPen, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
        foreach (var cell in Global.ListAllCellStruct)
        {
            if (cell.Border.Background == Brushes.Transparent)
            {
                graphics.FillRectangle(whiteBrush, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                var match = false;
                foreach (var word in Global.ListWordsGrid)
                {
                    if (cell.Label == word.ListLabel[0])
                    {
                        if (match == false)
                        {
                            count++;
                            match = true;
                        }

                        var text = count + ";";
                        foreach (var label in word.ListLabel)
                        {
                            text += label.Content.ToString();
                        }

                        if (word.Right)
                        {
                            listDefinitionRight.Add(text);
                        }
                        else
                        {
                            listDefinitionDown.Add(text);
                        }

                        graphics.DrawString(count.ToString(), font, blackBrush, (cell.X - topMaxX) * sizeCell, (cell.Y - leftMaxY) * sizeCell);
                    }
                }
            }
        }

        img.Save("EmptyGrid.png", ImageFormat.Png);
    }
}