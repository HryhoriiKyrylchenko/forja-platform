namespace Forja.Launcher.Models;

public class GameInstallation
{
    public Guid GameId { get; set; }
    public string InstalledVersion { get; set; } = "0.0.0";
    public string InstallPath { get; set; } = string.Empty;
    public string ExecutablePath { get; set; } = string.Empty;
    public List<InstalledFileInfo> InstalledFiles { get; set; } = [];

    public bool IsInstalled => !string.IsNullOrEmpty(ExecutablePath);
}