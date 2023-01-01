using System.Linq;

namespace Crossword.FormationOfAQueue;

public class SortingConnectionWords
{
    public static void Get()
    {
        foreach (var item in Global.listWordsGrid)
        {
            item.connectionWords = item.connectionWords.OrderByDescending(word => (float)word.connectionLabel.Count / word.listLabel.Count).ToList();
        }
    }
}