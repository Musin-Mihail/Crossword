using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class SearchForConnectedWords
{
    public static void Get()
    {
        foreach (var word in App.GameState.ListWordsGrid)
        {
            if (App.GameState.Stop) return;
            var tempListLabel = word.ListLabel;
            CreateWordDictionary.Get(word);
            foreach (var label in tempListLabel)
            {
                foreach (var word2 in App.GameState.ListWordsGrid)
                {
                    if (word != word2 && SearchForMatches.Get(word2, label))
                    {
                        if (word.ConnectionWords.Contains(word2) == false)
                        {
                            word.ConnectionWords.Add(word2);
                        }

                        if (word.ConnectionLabel.Contains(label) == false)
                        {
                            word.ConnectionLabel.Add(label);
                        }
                    }
                }
            }
        }
    }
}