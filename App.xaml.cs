using System;
using System.Windows;
using Crossword.Services;
using Crossword.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Crossword;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICrosswordStateService, CrosswordStateService>();

        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<GenerationService>();
        services.AddTransient<IDictionaryService, DictionaryService>();
        services.AddTransient<IScreenshotService, ScreenshotService>();
        services.AddSingleton<IGridManagerService, GridManagerService>();
        services.AddSingleton<MainWindow>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<ChangeFillViewModel>();
        services.AddTransient<DictionariesSelectionViewModel>();
        services.AddTransient<LoadGridViewModel>();
        services.AddTransient<RequiredDictionaryViewModel>();
        services.AddTransient<GenerationControlViewModel>();
        services.AddTransient<FileControlViewModel>();
        services.AddTransient<DictionaryControlViewModel>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}