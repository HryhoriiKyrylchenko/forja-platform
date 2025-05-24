namespace Forja.Application.DTOs.UserProfile;

public class UserAchievementDto
{
    public Guid Id { get; set; }
    public AchievementDto Achievement { get; set; } = null!;
    public Guid UserId { get; set; }
    public DateTime AchievedAt { get; set; }
}