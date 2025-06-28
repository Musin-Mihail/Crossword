using System.Windows;
using Crossword.ViewModel;

namespace Crossword;

public partial class ChangeFill : Window
{
    public ChangeFill(ChangeFillViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += (sender, result) =>
        {
            DialogResult = result;
            Close();
        };
    }
}