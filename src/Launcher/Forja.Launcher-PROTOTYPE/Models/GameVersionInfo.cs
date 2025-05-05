namespace Forja.Launcher.Models;

public class GameVersionInfo
{
    public Guid Id { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Changelog { get; set; } = string.Empty;
    public List<VersionFileInfo> Files { get; set; } = [];
}