using System;
using System.Windows;

namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get(DateTime startDate)
    {
        var time = DateTime.Now - startDate;
        App.GameState.StatusMessage = "ГЕНЕРАЦИЯ УДАЛАСЬ";
        var message = "";
        foreach (var dictionary in App.GameState.ListDictionaries)
        {
            message += $"\n{dictionary.Name} - {dictionary.CurrentCount}/{dictionary.MaxCount}";
            dictionary.CurrentCount = 0;
        }

        MessageBox.Show($"ГЕНЕРАЦИЯ УДАЛАСЬ\nза {time.TotalSeconds:F2} секунд\n{message}");
    }
}