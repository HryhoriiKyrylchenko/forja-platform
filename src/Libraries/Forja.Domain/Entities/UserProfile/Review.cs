namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents a review entity in the user profile schema.
/// </summary>
[Table("Reviews", Schema = "user-profile")]
public class Review : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the review.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the review.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }
        
    /// <summary>
    /// Gets or sets the unique identifier of the game being reviewed.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
        
    /// <summary>
    /// Gets or sets the rating given in the review. The rating must be between 1 and 5.
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; set; }
        
    /// <summary>
    /// Gets or sets the comment provided in the review. The comment can be up to 1000 characters long.
    /// </summary>
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
        
    /// <summary>
    /// Gets or sets the date and time when the review was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    /// <summary>
    /// Gets or sets a value indicating whether the review is approved.
    /// </summary>
    public bool IsApproved { get; set; } = false;
    
    /// <summary>
    /// Gets or sets the user who created the review.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the game being reviewed.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;
}