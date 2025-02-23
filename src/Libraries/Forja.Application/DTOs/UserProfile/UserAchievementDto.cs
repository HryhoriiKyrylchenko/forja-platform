namespace Forja.Application.DTOs.UserProfile;

public class UserAchievementDto
{
    public Guid Id { get; set; }
    public Achievement Achievement { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime AchievedAt { get; set; }
}