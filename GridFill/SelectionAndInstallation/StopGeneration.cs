using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StopGeneration
{
    public static void Get()
    {
        Global.gridGeneration.Visibility = Visibility.Visible;
        Global.startGeneration.Visibility = Visibility.Visible;
        Global.stopGeneration.Visibility = Visibility.Hidden;
    }
}