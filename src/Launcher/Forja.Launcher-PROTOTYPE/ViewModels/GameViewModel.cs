namespace Forja.Launcher.ViewModels;

public class GameViewModel : ReactiveObject
{
    public LibraryGameInfo ApiData { get; }
    public GameInstallation LocalData { get; }
    public string CurrentPlatform { get; }

    public string Title => ApiData.Title;
    public string LogoUrl => ApiData.LogoUrl;
    public Bitmap? LogoBitmap { get; private set; }

    public bool IsAvailableOnThisPlatform =>
        ApiData.Versions.Any(v => v.Platform.Equals(CurrentPlatform, StringComparison.OrdinalIgnoreCase));

    public GameVersionInfo? LatestVersion =>
        ApiData.Versions
            .Where(v => v.Platform.Equals(CurrentPlatform, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(v => v.Version)
            .FirstOrDefault();

    public bool IsInstalled => LocalData.IsInstalled;
    public bool RequiresUpdate => LatestVersion != null && LatestVersion.Version != LocalData.InstalledVersion;

    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set => this.RaiseAndSetIfChanged(ref _isRunning, value);
    }

    private double _progress;
    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }

    public GameViewModel(LibraryGameInfo apiData, GameInstallation localData, string currentPlatform)
    {
        ApiData = apiData;
        LocalData = localData;
        CurrentPlatform = currentPlatform;
    }

    public async Task LoadLogoAsync()
    {
        if (!string.IsNullOrEmpty(LogoUrl))
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(LogoUrl);

                if (response.IsSuccessStatusCode)
                {
                    await using var stream = await response.Content.ReadAsStreamAsync();
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        LogoBitmap = new Bitmap(stream);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load logo: {ex.Message}");
            }
        }
    }
}