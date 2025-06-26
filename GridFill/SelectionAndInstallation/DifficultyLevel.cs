namespace Crossword.GridFill.SelectionAndInstallation;

public class DifficultyLevel
{
    public static float Get()
    {
        float difficultyLevel = 0;
        foreach (var word in Global.ListWordsGrid)
        {
            difficultyLevel += (float)word.ConnectionLabel.Count / word.ListLabel.Count;
        }

        return difficultyLevel / Global.ListWordsGrid.Count;
    }
}