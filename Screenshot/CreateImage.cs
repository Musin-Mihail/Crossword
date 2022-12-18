﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Crossword.Words;

namespace Crossword.Screenshot;

public class CreateImage
{
    public static void Get(List<Cell> listCell, List<Word> listWord)
    {
        
        List<string> listDefinitionRight = new List<string>();
        List<string> listDefinitionDown = new List<string>();
        
        int topMaxX = 99;
        int leftMaxY = 99;
        int downMaxX = 99;
        int rightMaxY = 99;
        float sizeCell = 37.938105f;
        
        MaxCoordinateSearch.Get(listCell, ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY);
        int width = (int)((downMaxX - topMaxX + 1) * sizeCell);
        int height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
        Bitmap img = new Bitmap(width, height);
        img.SetResolution(300, 300);
        Graphics graphics = Graphics.FromImage(img);
        CreateEmptyGrid.Get(img, graphics, listCell, listWord, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown);
        CreateFillGrid.Get(img, graphics, listCell, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell);
        CreateAnswer.Get(listDefinitionRight, listDefinitionDown);
        CreateDefinition.Get(listDefinitionRight, listDefinitionDown);
    }
}