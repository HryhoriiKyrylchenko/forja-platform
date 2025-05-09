namespace Forja.Launcher.Models;

public class LibraryAddonModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PlatformType> Platforms { get; set; } = [];
    public List<ProductVersionModel> Versions { get; set; } = [];
}