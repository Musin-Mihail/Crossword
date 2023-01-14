using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get()
    {
        Global.windowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
        MessageBox.Show("ГЕНЕРАЦИЯ УДАЛАСЬ");
    }
}