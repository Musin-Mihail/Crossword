using Crossword.PlayingField;

namespace Crossword.Main;

public class ResetDict
{
    public static void Get()
    {
        Global.ListDictionaries.Clear();
        var commonDictionary = CreateDictionary.Get("dict.txt");
        Global.ListDictionaries.Add(commonDictionary);
        Global.ListDictionaries[^1].Name = "Общий";
        Global.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
        Global.selectedDictionary.Content = "Основной словарь";
    }
}