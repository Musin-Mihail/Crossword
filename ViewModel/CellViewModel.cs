using System.Windows.Media;

namespace Crossword.ViewModel;

public class CellViewModel : ViewModelBase
{
    private Brush _background = Brushes.Black;
    private string? _content;
    private int _x;
    private int _y;

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

    public int DisplayX => X * CellSize;
    public int DisplayY => Y * CellSize;

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
    public int Width => CellSize;
    public int Height => CellSize;
}