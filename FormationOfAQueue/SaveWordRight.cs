﻿using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class SaveWordRight
{
    public static void Get(int x, int y)
    {
        List<Label> newListLabel = new List<Label>();
        for (int i = x; i < 31; i++)
        {
            bool match = false;
            foreach (Cell cell in Global.listEmptyCellStruct)
            {
                if (cell.y == y && cell.x == i)
                {
                    newListLabel.Add(cell.label);
                    match = true;
                    break;
                }
            }

            if (match == false)
            {
                break;
            }
        }

        if (newListLabel.Count > 1)
        {
            Word newWord = new Word();
            newWord.listLabel = newListLabel;
            newWord.right = true;
            Global.listWordsGrid.Add(newWord);
        }
    }
}