namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to update a user's achievement information in the system.
/// </summary>
public class UserAchievementUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the user achievement update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the achievement to be associated with a user.
    /// </summary>
    [Required]
    public Guid AchievementId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the achievement.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the achievement was completed or attained.
    /// </summary>
    [Required]
    public DateTime AchievedAt { get; set; }
}