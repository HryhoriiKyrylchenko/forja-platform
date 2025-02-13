namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents an achievement entity in the user profile domain.
/// </summary>
[Table("Achievements", Schema = "user-profile")]
public class Achievement : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the achievement.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated game.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the name of the achievement.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the achievement.
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the points awarded for achieving this achievement.
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the achievement.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated game entity.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of user achievements associated with this achievement.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = [];
}