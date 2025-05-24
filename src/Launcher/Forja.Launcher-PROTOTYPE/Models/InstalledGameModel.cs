namespace Forja.Launcher.Models;

public class InstalledGameModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public PlatformType Platform { get; set; }
    public string InstallPath { get; set; } = string.Empty;
    public string? InstalledVersion { get; set; } = string.Empty;
    public List<InstalledAddonModel> InstalledAddons { get; set; } = [];
    public List<GamePatchModel> AvailablePatches { get; set; } = [];
    public List<InstalledFileModel> Files { get; set; } = [];
}