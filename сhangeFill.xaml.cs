using System.Windows;

namespace Crossword;

public partial class сhangeFill : Window
{
    public bool Ready;
    public int NumberOfCellsHorizontally = 30;
    public int NumberOfCellsVertically = 30;

    public сhangeFill()
    {
        InitializeComponent();
    }

    private void Button_ClickGen(object sender, RoutedEventArgs e)
    {
        var height = int.Parse(Horizontally.Text);
        var width = int.Parse(Vertically.Text);
        if (height > 30)
        {
            NumberOfCellsHorizontally = 30;
        }
        else
        {
            NumberOfCellsHorizontally = height;
        }

        if (width > 30)
        {
            NumberOfCellsVertically = 30;
        }
        else
        {
            NumberOfCellsVertically = width;
        }

        Ready = true;
        DialogResult = false;
    }
}