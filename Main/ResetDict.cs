using System.Windows.Controls;
using Crossword.PlayingField;

namespace Crossword.Main;

public class ResetDict
{
    public static void Get(Label selectedDictionary)
    {
        App.GameState.ListDictionaries.Clear();
        var commonDictionary = CreateDictionary.Get("dict.txt");
        App.GameState.ListDictionaries.Add(commonDictionary);
        App.GameState.ListDictionaries[^1].Name = "Общий";
        App.GameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
        selectedDictionary.Content = "Основной словарь";
    }
}