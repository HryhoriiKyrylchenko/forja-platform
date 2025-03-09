namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents the data required to create a new achievement.
/// </summary>
public class AchievementCreateRequest
{
    /// <summary>
    /// The name of the new achievement.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the new achievement.
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The points awarded for achieving this.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Points { get; set; }
    
    /// <summary>
    /// The URL of the logo associated with the achievement (optional).
    /// </summary>
    [Url]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// The unique identifier of the game this achievement is associated with.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }
}