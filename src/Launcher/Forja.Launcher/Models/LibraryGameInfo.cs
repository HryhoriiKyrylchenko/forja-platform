namespace Forja.Launcher.Models;

public class LibraryGameInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public List<GamePatchInfo> Patches { get; set; } = [];
    public List<GameAddonInfo> Addons { get; set; } = [];
    public List<GameVersionInfo> Versions { get; set; } = [];
}