namespace Crossword.GridFill;

public class CheckDictionary
{
    public static bool Get(string nameDictionary)
    {
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            if (dictionary.Name == nameDictionary)
            {
                if (dictionary.CurrentCount >= dictionary.MaxCount)
                {
                    return false;
                }

                return true;
            }
        }

        return true;
    }
}