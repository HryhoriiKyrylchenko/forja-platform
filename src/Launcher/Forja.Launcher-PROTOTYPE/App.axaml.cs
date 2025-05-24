namespace Forja.Launcher;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Initialize the dependency injection container
            var services = new ServiceCollection();
            services.AddSingleton<ApiService>();
            services.AddSingleton<GameLaunchService>();
            services.AddSingleton<GameInstallationService>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
        
            // Set the ServiceProvider so it can be accessed by the MainWindow
            ServiceProvider = services.BuildServiceProvider();
            
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var mainWindowViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();

            // Pass the MainWindowViewModel to the MainWindow constructor
            var mainWindow = new MainWindow(mainWindowViewModel);
            
            desktop.MainWindow = mainWindow;
        }
        
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}