using System.Windows;

namespace Crossword;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static CrosswordState GameState { get; } = new();
}