namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents the data needed to update an achievement.
/// </summary>
public class AchievementUpdateRequest
{
    /// <summary>
    /// The unique identifier of the achievement to be updated.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// The updated name of the achievement.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The updated description of the achievement.
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The updated points awarded for achieving this achievement.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Points { get; set; }

    /// <summary>
    /// The updated URL of the logo associated with the achievement (optional).
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// The unique identifier of the associated game.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }
}
