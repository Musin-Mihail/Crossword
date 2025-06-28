using System;
using Crossword.Infrastructure;

namespace Crossword.ViewModels;

public class ChangeFillViewModel : ViewModelBase
{
    private string _horizontal = "30";

    public string Horizontal
    {
        get => _horizontal;
        set => SetProperty(ref _horizontal, value);
    }

    private string _vertical = "30";

    public string Vertical
    {
        get => _vertical;
        set => SetProperty(ref _vertical, value);
    }

    public int ResultHorizontal { get; private set; }
    public int ResultVertical { get; private set; }

    public RelayCommand AcceptCommand { get; }

    public event EventHandler<bool>? CloseRequested;

    public ChangeFillViewModel()
    {
        AcceptCommand = new RelayCommand(Accept);
    }

    private void Accept(object? parameter)
    {
        if (int.TryParse(Horizontal, out var h) && int.TryParse(Vertical, out var v))
        {
            ResultHorizontal = h > 30 ? 30 : h;
            ResultVertical = v > 30 ? 30 : v;
            CloseRequested?.Invoke(this, true);
        }
        else
        {
            // Можно добавить логику для отображения ошибки, если введены не числа.
            // Например, через тот же IDialogService.
        }
    }
}