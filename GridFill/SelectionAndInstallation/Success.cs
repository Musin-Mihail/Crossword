using System;
using System.Windows;
using System.Windows.Controls;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get(DateTime startDate, Label windowsTextTop)
    {
        var time = DateTime.Now - startDate;
        windowsTextTop.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
        var message = "";
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            message += "\n" + dictionary.Name + " - " + dictionary.CurrentCount + "/" + dictionary.MaxCount;
            dictionary.CurrentCount = 0;
        }

        MessageBox.Show("ГЕНЕРАЦИЯ УДАЛАСЬ\n" + "за " + Math.Round((decimal)time.TotalSeconds, 2) + " секунд\n" + message);
    }
}