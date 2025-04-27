namespace Forja.Launcher.Models;

public class Game : ReactiveObject
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
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
}