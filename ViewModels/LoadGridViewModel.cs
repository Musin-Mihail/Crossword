using System;
using System.Collections.ObjectModel;
using System.IO;
using Crossword.Infrastructure;

namespace Crossword.ViewModels;

public class LoadGridViewModel : ViewModelBase
{
    public ObservableCollection<GridPreviewViewModel> SavedGrids { get; } = new();
    public string[] SelectedGridContent { get; private set; } = Array.Empty<string>();
    public event EventHandler<bool>? CloseRequested;

    public LoadGridViewModel()
    {
        LoadSavedGrids();
    }

    private void LoadSavedGrids()
    {
        SavedGrids.Clear();
        var saveDirectory = "SaveGrid";
        if (!Directory.Exists(saveDirectory)) return;

        var gridFiles = Directory.GetFiles(saveDirectory, "*.grid");
        foreach (var file in gridFiles)
        {
            SavedGrids.Add(new GridPreviewViewModel(file, OnLoadGrid, OnDeleteGrid));
        }
    }

    private void OnLoadGrid(GridPreviewViewModel gridVm)
    {
        SelectedGridContent = File.ReadAllLines(gridVm.FilePath);
        CloseRequested?.Invoke(this, true);
    }

    private void OnDeleteGrid(GridPreviewViewModel gridVm)
    {
        try
        {
            File.Delete(gridVm.FilePath);
            SavedGrids.Remove(gridVm);
        }
        catch (Exception)
        {
            // Обработка ошибки удаления файла
        }
    }
}