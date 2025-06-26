using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class SaveWordRight
{
    public static void Get(int x, int y)
    {
        var newListLabel = new List<Label>();
        for (var i = x; i < 31; i++)
        {
            var match = false;
            foreach (var cell in Global.ListEmptyCellStruct)
            {
                if (cell.Y == y && cell.X == i)
                {
                    newListLabel.Add(cell.Label);
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
            var newWord = new Word
            {
                ListLabel = newListLabel,
                Right = true
            };
            Global.ListWordsGrid.Add(newWord);
        }
    }
}