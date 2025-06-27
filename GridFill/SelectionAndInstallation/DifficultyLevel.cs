namespace Crossword.GridFill.SelectionAndInstallation;

public class DifficultyLevel
{
    public static float Get()
    {
        float difficultyLevel = 0;
        foreach (var word in App.GameState.ListWordsGrid)
        {
            difficultyLevel += (float)word.ConnectionLabel.Count / word.ListLabel.Count;
        }

        return difficultyLevel / App.GameState.ListWordsGrid.Count;
    }
}