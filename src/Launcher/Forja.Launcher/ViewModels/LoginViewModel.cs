namespace Forja.Launcher.ViewModels;

public class LoginViewModel : ViewModelBase, IReactiveObject
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public event Action? LoginSucceeded;

    private readonly ApiService _apiService;
    
    public new event PropertyChangingEventHandler? PropertyChanging;
    public new event PropertyChangedEventHandler? PropertyChanged;

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;

        LoginCommand = ReactiveCommand.CreateFromTask(
            async () => await LoginAsync(),
            outputScheduler: AvaloniaScheduler.Instance 
        );
    }

    private async Task LoginAsync()
    {
        try
        {
            var result = await _apiService.LoginAsync(Email, Password);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (result)
                {
                    LoginSucceeded?.Invoke();  
                }
                else
                {
                    ErrorMessage = "Login failed!";
                }
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ErrorMessage = $"Error: {ex.Message}";
            });
        }
    }

    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        PropertyChanging?.Invoke(this, args);
    }

    public void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }
}