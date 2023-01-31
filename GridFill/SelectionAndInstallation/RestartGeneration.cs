namespace Crossword.GridFill.SelectionAndInstallation;

public class RestartGeneration
{
    public static void Get()
    {
        Global.index = 0;
        ClearAllCell.Get();
        foreach (var dictionary in Global.listDictionaries)
        {
            dictionary.currentCount = 0;
        }
    }
}