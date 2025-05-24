namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a mapping between a game and a specific game mechanic.
/// </summary>
/// <remarks>
/// This class captures the relationship between the Game and Mechanic entities in the database.
/// </remarks>
[Table("GameMechanics", Schema = "games")]
public class GameMechanic
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the associated <see cref="Game"/> entity.
    /// This property establishes a relationship between the <see cref="GameMechanic"/> and the corresponding game.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Represents the unique identifier of the associated mechanic in a game.
    /// This property is used as a foreign key to establish a relationship with the Mechanic entity.
    /// </summary>
    [ForeignKey("ItemImage")]
    public Guid MechanicId { get; set; }

    /// <summary>
    /// Represents a game entity with associated properties and relationships within the application.
    /// </summary>
    /// <remarks>
    /// This entity inherits from the base class Product and extends its functionality to accommodate game-specific attributes
    /// such as system requirements, storage URL, and associated collections for categories, tags, addons, reviews, and mechanics.
    /// </remarks>
    public virtual Game Game { get; set; } = null!;

    /// <summary>
    /// Represents a game mechanic, including its name, description, associated logo,
    /// and the collection of games with which it is linked.
    /// </summary>
    public virtual Mechanic Mechanic { get; set; } = null!;
}