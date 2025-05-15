namespace Forja.Launcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private ViewModelBase? _currentViewModel;
    
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        var isLoggedIn = false;

        if (isLoggedIn)
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        }
        else
        {
            var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginViewModel.LoginSucceeded += OnLoginSucceeded;
            CurrentViewModel = loginViewModel;
        }
    }
    
    private void OnLoginSucceeded()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        });
    }
}