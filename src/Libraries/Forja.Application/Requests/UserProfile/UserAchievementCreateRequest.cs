namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to create a user achievement.
/// </summary>
public class UserAchievementCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the achievement to be associated with a user.
    /// </summary>
    public Guid AchievementId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the achievement.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the achievement was completed or attained.
    /// </summary>
    public DateTime AchievedAt { get; set; }
}