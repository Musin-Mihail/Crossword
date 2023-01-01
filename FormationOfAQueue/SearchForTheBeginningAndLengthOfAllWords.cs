using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class SearchForTheBeginningAndLengthOfAllWords
{
    public static void Get()
    {
        foreach (Cell cell in Global.listEmptyCellStruct)
        {
            int x = cell.x;
            int y = cell.y;
            bool black = true;

            foreach (Cell cell2 in Global.listEmptyCellStruct)
            {
                if (cell2.x == x - 1 && cell2.y == y)
                {
                    black = false;
                    break;
                }
            }

            if (black == true)
            {
                SaveWordRight.Get(x, y);
            }

            black = true;
            foreach (Cell cell2 in Global.listEmptyCellStruct)
            {
                if (cell2.x == x && cell2.y == y - 1)
                {
                    black = false;
                    break;
                }
            }

            if (black == true)
            {
                SaveWordDown.Get(x, y);
            }
        }
    }
}