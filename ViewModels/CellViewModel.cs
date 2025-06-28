using System.Windows.Media;
using Crossword.Infrastructure;

namespace Crossword.ViewModels;

public class CellViewModel : ViewModelBase
{
    private Brush _background = Brushes.Black;
    private string? _content;
    private int _x;
    private int _y;
    public bool IsPreview { get; set; }

    public int X
    {
        get => _x;
        set => SetProperty(ref _x, value);
    }

    public int Y
    {
        get => _y;
        set => SetProperty(ref _y, value);
    }

    public int DisplayX => IsPreview ? (X - 1) * PreviewCellSize : X * CellSize;
    public int DisplayY => IsPreview ? (Y - 1) * PreviewCellSize : Y * CellSize;

    public Brush Background
    {
        get => _background;
        set => SetProperty(ref _background, value);
    }

    public string? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public static int CellSize => 30;
    public static int PreviewCellSize => 5;
    public int Width => IsPreview ? PreviewCellSize : CellSize;
    public int Height => IsPreview ? PreviewCellSize : CellSize;
}