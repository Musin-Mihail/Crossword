using System;
using System.Windows;

namespace Crossword;

public partial class сhangeFill : Window
{
    public bool ready = false;
    public int numberOfCellsHorizontally = 30;
    public int numberOfCellsVertically = 30;

    public сhangeFill()
    {
        InitializeComponent();
    }

    private void Button_ClickGen(object sender, RoutedEventArgs e)
    {
        int height = Int32.Parse(Horizontally.Text);
        int width = Int32.Parse(Vertically.Text);
        if (height > 30)
        {
            numberOfCellsHorizontally = 30;
        }
        else
        {
            numberOfCellsHorizontally = height;
        }

        if (width > 30)
        {
            numberOfCellsVertically = 30;
        }
        else
        {
            numberOfCellsVertically = width;
        }

        ready = true;
        DialogResult = false;
    }
}