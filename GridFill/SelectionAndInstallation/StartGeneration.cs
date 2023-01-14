using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class StartGeneration
{
    public static void Get()
    {
        Global.gridGeneration.Visibility = Visibility.Hidden;
        Global.startGeneration.Visibility = Visibility.Hidden;
        Global.stopGeneration.Visibility = Visibility.Visible;
    }
}