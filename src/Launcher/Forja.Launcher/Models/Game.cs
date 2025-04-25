namespace Forja.Launcher.Models;

public class Game
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LocalVersion { get; set; } = "0.0.0";
    public string LatestVersion { get; set; } = "0.0.0";
    public string ExecutablePath { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
}