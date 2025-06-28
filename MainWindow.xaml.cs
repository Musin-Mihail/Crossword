using System.Windows;
using System.Windows.Input;
using Crossword.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword;

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
                if (vm.CellInteractionCommand.CanExecute(cellVm))
                {
                    vm.CellInteractionCommand.Execute(cellVm);
                }
            }
        }
    }
}