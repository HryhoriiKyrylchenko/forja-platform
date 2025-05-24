namespace Forja.Launcher.Models;

public class LibraryGameModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public List<PlatformType> Platforms { get; set; } = [];
    public List<GamePatchModel> Patches { get; set; } = [];
    public List<LibraryAddonModel> Addons { get; set; } = [];
    public List<ProductVersionModel> Versions { get; set; } = [];
}