using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword.Screenshot;

public class CreateImage
{
    public static void Get()
    {
        var gameState = App.ServiceProvider.GetRequiredService<CrosswordState>();
        try
        {
            var topMaxX = 99;
            var leftMaxY = 99;
            var downMaxX = 99;
            var rightMaxY = 99;
            const float sizeCell = 37.938105f;
            MaxCoordinateSearch.Get(ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY, gameState);
            var width = (int)((downMaxX - topMaxX + 1) * sizeCell);
            var height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
            var img = new Bitmap(width, height);
            img.SetResolution(300, 300);
            var graphics = Graphics.FromImage(img);
            var listDefinitionRight = new List<string>();
            var listDefinitionDown = new List<string>();
            CreateEmptyGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown, gameState);
            CreateFillGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, gameState);
            CreateAnswer.Get(listDefinitionRight, listDefinitionDown);
            CreateDefinition.Get(listDefinitionRight, listDefinitionDown, gameState);
            MessageBox.Show("Кросворд сохранён");
        }
        catch (Exception e)
        {
            MessageBox.Show("CreateImage\n" + e);
            throw;
        }
    }
}