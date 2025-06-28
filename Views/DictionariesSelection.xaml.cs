using System.Windows;
using Crossword.ViewModels;

namespace Crossword.Views;

public partial class DictionariesSelection : Window
{
    public DictionariesSelection(DictionariesSelectionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += () => { DialogResult = true; };
    }
}