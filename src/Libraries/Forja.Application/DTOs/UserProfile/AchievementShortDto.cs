namespace Forja.Application.DTOs.UserProfile;

public class AchievementShortDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; } = string.Empty;
}