using System.Windows;
using Crossword.ViewModels;

namespace Crossword.Views;

public partial class LoadGrid : Window
{
    public LoadGrid(LoadGridViewModel viewModel)
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