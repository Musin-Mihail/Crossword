using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Crossword.ViewModel;

public class DictionaryInfoViewModel : INotifyPropertyChanged
{
    private string _selectedWordCount = "0";

    public string Name { get; set; }
    public int TotalWordCount { get; set; }

    public string SelectedWordCount
    {
        get => _selectedWordCount;
        set
        {
            if (value.All(char.IsDigit))
            {
                _selectedWordCount = value;
                OnPropertyChanged();
            }
        }
    }

    public DictionaryInfoViewModel(string name, int totalWordCount)
    {
        Name = name;
        TotalWordCount = totalWordCount;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}