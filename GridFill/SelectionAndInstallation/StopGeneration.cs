using System.Windows;
using System.Windows.Controls;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StopGeneration
{
    public static void Get(Grid gridGeneration, Button genStartButton, Button genStopButton)
    {
        gridGeneration.Visibility = Visibility.Visible;
        genStartButton.Visibility = Visibility.Visible;
        genStopButton.Visibility = Visibility.Hidden;
    }
}