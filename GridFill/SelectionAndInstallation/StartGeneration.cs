using System.Windows;
using System.Windows.Controls;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StartGeneration
{
    public static void Get(Grid gridGeneration, Button genStartButton, Button genStopButton)
    {
        gridGeneration.Visibility = Visibility.Hidden;
        genStartButton.Visibility = Visibility.Hidden;
        genStopButton.Visibility = Visibility.Visible;
    }
}