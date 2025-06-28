using System;
using System.Windows;
using Crossword.Services;
using Crossword.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> for the application.
    /// </summary>
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<GenerationService>();
        services.AddTransient<IDictionaryService, DictionaryService>();
        services.AddTransient<IScreenshotService, ScreenshotService>();
        services.AddSingleton<MainWindow>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<ChangeFillViewModel>();
        services.AddTransient<DictionariesSelectionViewModel>();
        services.AddTransient<LoadGridViewModel>();
        services.AddTransient<RequiredDictionaryViewModel>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}