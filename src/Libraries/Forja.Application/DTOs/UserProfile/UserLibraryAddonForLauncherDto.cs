namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryAddonForLauncherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PlatformType> Platforms { get; set; } = [];
    public List<ProductVersionDto> Versions { get; set; } = [];
}