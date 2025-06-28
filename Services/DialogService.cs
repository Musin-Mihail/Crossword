using System;
using System.Collections.Generic;
using System.Windows;
using Crossword.Objects;

namespace Crossword.Services;

public class DialogService : IDialogService
{
    public bool? ShowChangeFillDialog(ref int horizontal, ref int vertical)
    {
        var dialog = new ChangeFill();
        var result = dialog.ShowDialog();
        if (dialog.Ready)
        {
            horizontal = dialog.NumberOfCellsHorizontally;
            vertical = dialog.NumberOfCellsVertically;
        }

        return result;
    }

    public bool? ShowLoadGridDialog(out string[] listEmptyCellStruct)
    {
        var dialog = new LoadGrid();
        var result = dialog.ShowDialog();
        listEmptyCellStruct = dialog.Ready ? dialog.ListEmptyCellStruct : Array.Empty<string>();
        return result;
    }

    public bool? ShowDictionariesSelectionDialog(out List<string> selectedDictionaries)
    {
        var dialog = new DictionariesSelection();
        var result = dialog.ShowDialog();
        selectedDictionaries = dialog.Ready ? dialog.SelectedDictionaries : new List<string>();
        return result;
    }

    public void ShowRequiredDictionaryDialog(List<Dictionary> availableDictionaries)
    {
        new RequiredDictionary(availableDictionaries).ShowDialog();
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}