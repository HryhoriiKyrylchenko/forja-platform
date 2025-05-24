namespace Forja.Launcher.Models;

public class InstalledAddonModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InstalledVersion { get; set; } = string.Empty;
    public List<InstalledFileModel> Files { get; set; } = [];
}