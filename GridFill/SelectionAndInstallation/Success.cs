using System;
using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get(DateTime startDate)
    {
        var time = DateTime.Now - startDate;
        Global.windowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
        var message = "";
        foreach (var dictionary in Global.ListDictionaries)
        {
            message += "\n" + dictionary.Name + " - " + dictionary.CurrentCount + "/" + dictionary.MaxCount;
            dictionary.CurrentCount = 0;
        }

        MessageBox.Show("ГЕНЕРАЦИЯ УДАЛАСЬ\n" + "за " + Math.Round((decimal)time.TotalSeconds, 2) + " секунд\n" + message);
    }
}