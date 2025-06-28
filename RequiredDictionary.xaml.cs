using System.Windows;
using Crossword.ViewModel;

namespace Crossword;

public partial class RequiredDictionary : Window
{
    public RequiredDictionary(RequiredDictionaryViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += (sender, e) => { Close(); };
    }
}