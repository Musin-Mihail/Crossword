using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace Crossword.Screenshot;

public class CreateImage
{
    public static void Get()
    {
        try
        {
            var topMaxX = 99;
            var leftMaxY = 99;
            var downMaxX = 99;
            var rightMaxY = 99;
            const float sizeCell = 37.938105f;
            MaxCoordinateSearch.Get(ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY);
            var width = (int)((downMaxX - topMaxX + 1) * sizeCell);
            var height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
            var img = new Bitmap(width, height);
            img.SetResolution(300, 300);
            var graphics = Graphics.FromImage(img);
            var listDefinitionRight = new List<string>();
            var listDefinitionDown = new List<string>();
            CreateEmptyGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown);
            CreateFillGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell);
            CreateAnswer.Get(listDefinitionRight, listDefinitionDown);
            CreateDefinition.Get(listDefinitionRight, listDefinitionDown);
            MessageBox.Show("Кросворд сохранён");
        }
        catch (Exception e)
        {
            MessageBox.Show("CreateImage\n" + e);
            throw;
        }
    }
}