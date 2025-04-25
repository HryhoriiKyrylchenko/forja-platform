namespace Forja.Launcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IReactiveObject
{
    private readonly ApiService _apiService;
    public ViewModelBase CurrentViewModel { get; private set; }

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
        CurrentViewModel = new MainViewModel(_apiService);
        this.RaisePropertyChanged(nameof(CurrentViewModel));
    }
    
    public new event PropertyChangingEventHandler? PropertyChanging;
    public new event PropertyChangedEventHandler? PropertyChanged;

    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        PropertyChanging?.Invoke(this, args);
    }

    public void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }
}