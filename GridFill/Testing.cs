using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Media;
using Crossword.Words;

namespace Crossword.GridFill;

public class Testing
{
    public static void Get(List<Cell> listEmptyCellStruct, List<Word> listWordStruct)
    {
        foreach (var cell in listEmptyCellStruct)
        {
            cell.border.Background = Brushes.Aqua;
        }
        foreach (var word in listWordStruct)
        {
            foreach (var label in word.listLabel)
            {
                label.Content = "O";
            }
        }
    }
}