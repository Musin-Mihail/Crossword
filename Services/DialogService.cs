using System;
using System.Collections.Generic;
using System.Windows;
using Crossword.Objects;
using Crossword.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword.Services;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool? ShowChangeFillDialog(ref int horizontal, ref int vertical)
    {
        var viewModel = _serviceProvider.GetRequiredService<ChangeFillViewModel>();
        viewModel.Horizontal = horizontal.ToString();
        viewModel.Vertical = vertical.ToString();
        var dialog = new ChangeFill(viewModel);
        var result = dialog.ShowDialog();
        if (result == true)
        {
            horizontal = viewModel.ResultHorizontal;
            vertical = viewModel.ResultVertical;
        }

        return result;
    }

    public bool? ShowLoadGridDialog(out string[] listEmptyCellStruct)
    {
        var viewModel = _serviceProvider.GetRequiredService<LoadGridViewModel>();
        var dialog = new LoadGrid(viewModel);
        var result = dialog.ShowDialog();
        listEmptyCellStruct = result == true ? viewModel.SelectedGridContent : Array.Empty<string>();
        return result;
    }

    public bool? ShowDictionariesSelectionDialog(out List<string> selectedDictionaries)
    {
        var viewModel = _serviceProvider.GetRequiredService<DictionariesSelectionViewModel>();
        var dialog = new DictionariesSelection(viewModel);
        var result = dialog.ShowDialog();
        if (result == true)
        {
            selectedDictionaries = viewModel.SelectionResult;
        }
        else
        {
            selectedDictionaries = new List<string>();
        }

        return result;
    }

    public void ShowRequiredDictionaryDialog(List<Dictionary> availableDictionaries)
    {
        var viewModel = _serviceProvider.GetRequiredService<RequiredDictionaryViewModel>();
        viewModel.Initialize(availableDictionaries);
        var dialog = new RequiredDictionary(viewModel);
        dialog.ShowDialog();
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}