namespace Forja.Launcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IReactiveObject
{
    private readonly ApiService _apiService;
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

    public MainWindowViewModel(ApiService apiService)
    {
        _apiService = apiService;
        var isLoggedIn = false;

        if (isLoggedIn)
        {
            CurrentViewModel = new MainViewModel(apiService);
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
            CurrentViewModel = new MainViewModel(_apiService);
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