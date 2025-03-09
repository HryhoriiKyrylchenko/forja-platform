namespace Forja.Application.DTOs.UserProfile;

public class UserAchievementDto
{
    public Guid Id { get; set; }
    public AchievementDto Achievement { get; set; } = null!;
    public UserProfileDto User { get; set; } = null!;
    public DateTime AchievedAt { get; set; }
}