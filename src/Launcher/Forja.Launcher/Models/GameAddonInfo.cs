namespace Forja.Launcher.Models;

public class GameAddonInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
}