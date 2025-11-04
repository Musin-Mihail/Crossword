using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using Crossword.Infrastructure;

namespace Crossword.ViewModels;

public class GridPreviewViewModel : ViewModelBase
{
    public string FilePath { get; }
    public ObservableCollection<CellViewModel> PreviewCells { get; } = new();
    public ICommand LoadCommand { get; }
    public ICommand DeleteCommand { get; }

    public GridPreviewViewModel(string filePath, Action<GridPreviewViewModel> loadAction, Action<GridPreviewViewModel> deleteAction)
    {
        FilePath = filePath;
        LoadCommand = new RelayCommand(_ => loadAction(this));
        DeleteCommand = new RelayCommand(_ => deleteAction(this));
        GeneratePreview();
    }

    private void GeneratePreview()
    {
        var lines = File.ReadAllLines(FilePath);
        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (parts.Length == 2 && int.TryParse(parts[0], out var x) && int.TryParse(parts[1], out var y))
            {
                PreviewCells.Add(new CellViewModel
                {
                    X = x, Y = y, Background = Brushes.White, IsPreview = true
                });
            }
        }
    }
}