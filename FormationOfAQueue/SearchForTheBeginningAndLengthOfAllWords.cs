using System.Collections.Generic;
using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class SearchForTheBeginningAndLengthOfAllWords
{
    public static void Get(List<Word> listWordStruct, List<Cell> listEmptyCellStruct)
    {
        foreach (Cell cell in listEmptyCellStruct)
        {
            int x = cell.x;
            int y = cell.y;
            bool black = true;

            foreach (Cell cell2 in listEmptyCellStruct)
            {
                if (cell2.x == x - 1 && cell2.y == y)
                {
                    black = false;
                    break;
                }
            }

            if (black == true)
            {
                SaveWordRight.Get(listWordStruct, listEmptyCellStruct, x, y);
            }

            black = true;
            foreach (Cell cell2 in listEmptyCellStruct)
            {
                if (cell2.x == x && cell2.y == y - 1)
                {
                    black = false;
                    break;
                }
            }

            if (black == true)
            {
                SaveWordDown.Get(listWordStruct, listEmptyCellStruct, x, y);
            }
        }
    }
}