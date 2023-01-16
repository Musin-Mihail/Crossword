using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get()
    {
        Global.windowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
        string message = "";
        foreach (var dictionary in Global.listDictionaries)
        {
            message += "\n" + dictionary.name + " - " + dictionary.currentCount + "/" + dictionary.maxCount;
            dictionary.currentCount = 0;
        }

        MessageBox.Show("ГЕНЕРАЦИЯ УДАЛАСЬ\n" + message);
    }
}