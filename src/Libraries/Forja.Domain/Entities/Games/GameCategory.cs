namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents the association between a game and a category.
/// </summary>
[Table("GameCategories", Schema = "games")]
public class GameCategory
{
    /// <summary>
    /// Gets or sets the unique identifier for the game category.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the associated game.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the associated category.
    /// </summary>
    [ForeignKey("Category")]
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Gets or sets the associated game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the associated category.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Category Category { get; set; } = null!;
}