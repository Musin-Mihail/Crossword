using Crossword.PlayingField;

namespace Crossword.Main;

public class ResetDict
{
    private readonly CrosswordState _gameState;

    public ResetDict(CrosswordState gameState)
    {
        _gameState = gameState;
    }

    public void Get()
    {
        _gameState.ListDictionaries.Clear();
        var commonDictionary = CreateDictionary.Get("dict.txt");
        _gameState.ListDictionaries.Add(commonDictionary);
        _gameState.ListDictionaries[^1].Name = "Общий";
        _gameState.ListDictionaries[^1].MaxCount = commonDictionary.Words.Count;
        _gameState.SelectedDictionaryInfo = "Основной словарь";
    }
}