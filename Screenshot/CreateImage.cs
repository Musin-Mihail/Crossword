using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace Crossword.Screenshot;

public class CreateImage
{
    public static void Get()
    {
        int topMaxX = 99;
        int leftMaxY = 99;
        int downMaxX = 99;
        int rightMaxY = 99;
        float sizeCell = 37.938105f;
        MaxCoordinateSearch.Get(ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY);
        int width = (int)((downMaxX - topMaxX + 1) * sizeCell);
        int height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
        Bitmap img = new Bitmap(width, height);
        img.SetResolution(300, 300);
        Graphics graphics = Graphics.FromImage(img);
        List<string> listDefinitionRight = new List<string>();
        List<string> listDefinitionDown = new List<string>();
        CreateEmptyGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown);
        CreateFillGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell);
        CreateAnswer.Get(listDefinitionRight, listDefinitionDown);
        CreateDefinition.Get(listDefinitionRight, listDefinitionDown);
        MessageBox.Show("Кросворд сохранён");
    }
}