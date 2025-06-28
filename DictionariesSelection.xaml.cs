using System.Windows;
using Crossword.ViewModel;

namespace Crossword;

public partial class DictionariesSelection : Window
{
    public DictionariesSelection(DictionariesSelectionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += () => { DialogResult = true; };
    }
}