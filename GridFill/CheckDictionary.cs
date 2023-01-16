namespace Crossword.GridFill;

public class CheckDictionary
{
    public static bool Get(string nameDictionary)
    {
        foreach (var dictionary in Global.listDictionaries)
        {
            if (dictionary.name == nameDictionary)
            {
                if (dictionary.currentCount >= dictionary.maxCount)
                {
                    return false;
                }

                return true;
            }
        }

        return true;
    }
}