namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents an achievement earned by a user.
/// </summary>
[Table("UserAchievements", Schema = "user-profile")]
public class UserAchievement
{
    /// <summary>
    /// Gets or sets the unique identifier for the user achievement.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier of the user who earned the achievement.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the achievement.
    /// </summary>
    [ForeignKey("Achievement")]
    public Guid AchievementId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the achievement was earned.
    /// </summary>
    public DateTime AchievedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the user who earned the achievement.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the achievement that was earned.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Achievement Achievement { get; set; } = null!;
}