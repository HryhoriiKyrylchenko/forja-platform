using Forja.Application.DTOs.Games;

namespace Forja.Application.DTOs.UserProfile;

public class AchievementDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Points { get; set; }
    public string? LogoUrl { get; set; } = string.Empty;
    public GameDto Game { get; set; } = null!;
}