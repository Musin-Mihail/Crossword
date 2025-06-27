using Crossword.Objects;

namespace Crossword.FormationOfAQueue;

public class SearchForTheBeginningAndLengthOfAllWords
{
    public static void Get()
    {
        foreach (Cell cell in App.GameState.ListEmptyCellStruct)
        {
            var x = cell.X;
            var y = cell.Y;
            var black = true;

            foreach (var cell2 in App.GameState.ListEmptyCellStruct)
            {
                if (cell2.X == x - 1 && cell2.Y == y)
                {
                    black = false;
                    break;
                }
            }

            if (black)
            {
                SaveWordRight.Get(x, y);
            }

            black = true;
            foreach (var cell2 in App.GameState.ListEmptyCellStruct)
            {
                if (cell2.X == x && cell2.Y == y - 1)
                {
                    black = false;
                    break;
                }
            }

            if (black)
            {
                SaveWordDown.Get(x, y);
            }
        }
    }
}