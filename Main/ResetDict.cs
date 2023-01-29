using Crossword.Objects;
using Crossword.PlayingField;

namespace Crossword.Main;

public class ResetDict
{
    public static void Get()
    {
        Global.listDictionaries.Clear();

        Dictionary commonDictionary = CreateDictionary.Get("dict.txt");
        Global.listDictionaries.Add(commonDictionary);
        Global.listDictionaries[^1].name = "Общий";
        Global.listDictionaries[^1].maxCount = commonDictionary.words.Count;

        Global.selectedDictionary.Content = "Основной словарь";
    }
}