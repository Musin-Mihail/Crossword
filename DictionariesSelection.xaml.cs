using System.Collections.Generic;
using System.Windows;
using Crossword.ViewModel;

namespace Crossword;

public partial class DictionariesSelection : Window
{
    public bool Ready { get; private set; }
    public List<string> SelectedDictionaries { get; private set; } = new();

    public DictionariesSelection()
    {
        InitializeComponent();
        var viewModel = new DictionariesSelectionViewModel();
        DataContext = viewModel;
        viewModel.CloseRequested += () =>
        {
            SelectedDictionaries = viewModel.SelectionResult;
            Ready = true;
            DialogResult = true;
        };
    }
}