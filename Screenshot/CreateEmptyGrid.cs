using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;
using Crossword.Words;

namespace Crossword.Screenshot;

public class CreateEmptyGrid
{
    public static void Get(Bitmap img, Graphics graphics, List<Cell> listCell, List<Word> listWord, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Pen blackPen = new Pen(Color.Black, 1);
        Font font = new Font("Arial", 4);
        graphics.Clear(Color.White);

        int count = 0;
        graphics.FillRectangle(blackBrush, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
        graphics.DrawRectangle(blackPen, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
        AddingWatermarks.Get(graphics);
        foreach (Cell cell in listCell)
        {
            if (cell.border.Background == Brushes.Transparent)
            {
                graphics.FillRectangle(whiteBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                bool match = false;
                foreach (Word word in listWord)
                {
                    if (cell.label == word.listLabel[0])
                    {
                        if (match == false)
                        {
                            count++;
                            match = true;
                        }
        
                        string text = count + ";";
                        foreach (Label label in word.listLabel)
                        {
                            text += label.Content.ToString();
                        }
        
                        if (word.right)
                        {
                            listDefinitionRight.Add(text);
                        }
                        else
                        {
                            listDefinitionDown.Add(text);
                        }
        
                        graphics.DrawString(count.ToString(), font, blackBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell);
                    }
                }
            }
        }

        img.Save("EmptyGrid.png", ImageFormat.Png);
    }
}