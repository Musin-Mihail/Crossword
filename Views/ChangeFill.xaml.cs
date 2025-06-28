using System.Windows;
using Crossword.ViewModels;

namespace Crossword.Views;

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