namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a tag associated with a game.
/// </summary>
[Table("GameTags", Schema = "games")]
public class GameTag
{
    /// <summary>
    /// Gets or sets the unique identifier for the game tag.
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
    /// Gets or sets the unique identifier for the associated tag.
    /// </summary>
    [ForeignKey("Tag")]
    public Guid TagId { get; set; }
    
    /// <summary>
    /// Gets or sets the game associated with this tag.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the tag associated with this game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Tag Tag { get; set; } = null!;
}