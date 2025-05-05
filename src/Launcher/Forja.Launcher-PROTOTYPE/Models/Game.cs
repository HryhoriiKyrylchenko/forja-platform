namespace Forja.Launcher.Models;

public class Game : ReactiveObject
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LocalVersion { get; set; } = "0.0.0";
    public string LatestVersion { get; set; } = "0.0.0";
    public string ExecutablePath { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;  
    public bool IsInstalled => !string.IsNullOrEmpty(ExecutablePath);
    public bool IsUpdated => LocalVersion == LatestVersion;
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
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
    
    private Bitmap? _logoBitmap;
    public Bitmap? LogoBitmap
    {
        get => _logoBitmap;
        private set => this.RaiseAndSetIfChanged(ref _logoBitmap, value);
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
                else
                {
                    Debug.WriteLine($"Failed to load logo for {Title}: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load logo for {Title}: {ex.Message}");
            }
        }
    }
    
    public List<Addon> InstalledAddons { get; set; } = [];
}