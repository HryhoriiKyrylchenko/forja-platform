namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryGameForLauncherDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public List<PlatformType> Platforms { get; set; } = [];
    public List<GamePatchDto> Patches { get; set; } = [];
    public List<UserLibraryAddonForLauncherDto> Addons { get; set; } = [];
    public List<ProductVersionDto> Versions { get; set; } = [];
}