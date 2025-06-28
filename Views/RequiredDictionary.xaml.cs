using System.Windows;
using Crossword.ViewModels;

namespace Crossword.Views;

public partial class RequiredDictionary : Window
{
    public RequiredDictionary(RequiredDictionaryViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += (sender, e) => { Close(); };
    }
}