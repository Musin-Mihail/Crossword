namespace Crossword.GridFill.SelectionAndInstallation;

public class DifficultyLevel
{
    public static float Get()
    {
        float difficultyLevel = 0;
        foreach (var word in Global.listWordsGrid)
        {
            difficultyLevel += (float)word.connectionLabel.Count / word.listLabel.Count;
        }

        return difficultyLevel / Global.listWordsGrid.Count;
    }
}