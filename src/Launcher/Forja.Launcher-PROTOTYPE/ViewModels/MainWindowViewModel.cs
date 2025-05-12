namespace Forja.Launcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IReactiveObject
{
    private readonly ApiService _apiService;
    private readonly GameInstallationService _gameInstallationService;
    private readonly GameLaunchService _gameLaunchService;
    private ViewModelBase? _currentViewModel;
    
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel != value)
            {
                _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel)); 
            }
        }
    }

    public MainWindowViewModel(ApiService apiService, 
        GameInstallationService gameInstallationService,
        GameLaunchService gameLaunchService)
    {
        _apiService = apiService;
        _gameInstallationService = gameInstallationService;
        _gameLaunchService = gameLaunchService;
        
        var isLoggedIn = false;

        if (isLoggedIn)
        {
            CurrentViewModel = new MainViewModel(apiService, gameInstallationService, gameLaunchService);
        }
        else
        {
            var loginViewModel = new LoginViewModel(apiService);
            loginViewModel.LoginSucceeded += OnLoginSucceeded;
            CurrentViewModel = loginViewModel;
        }
    }
    
    private void OnLoginSucceeded()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentViewModel = new MainViewModel(_apiService, _gameInstallationService, _gameLaunchService);
        });
    }
    
    public new event PropertyChangingEventHandler? PropertyChanging;
    public new event PropertyChangedEventHandler? PropertyChanged;

    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        PropertyChanging?.Invoke(this, args);
    }

    private void RaisePropertyChanged(string propertyName)  
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
    }
}