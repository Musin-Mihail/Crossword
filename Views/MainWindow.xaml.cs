using System.Windows;
using System.Windows.Input;
using Crossword.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
    }

    private void Cell_Interaction(object sender, MouseEventArgs e)
    {
        if (DataContext is MainViewModel vm && sender is FrameworkElement element)
        {
            if (element.DataContext is CellViewModel cellVm)
            {
                if (vm.GridControls.CellInteractionCommand.CanExecute(cellVm))
                {
                    vm.GridControls.CellInteractionCommand.Execute(cellVm);
                }
            }
        }
    }
}