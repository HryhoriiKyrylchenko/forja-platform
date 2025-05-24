namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a game mechanic, providing details about its name, description, logo, and associated games.
/// </summary>
[Table("Mechanics", Schema = "games")]
public class Mechanic : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the mechanic.
    /// This represents a unique and concise name for the game mechanic,
    /// limited to a maximum length of 50 characters.
    /// The property is required and cannot be empty.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents a description of the mechanic entity, providing detailed information
    /// about the nature or functionality of the mechanic within a game.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the logo associated with the mechanic.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the association between games and their respective mechanics.
    /// </summary>
    /// <remarks>
    /// This property facilitates the many-to-many relationship between the <c>Game</c> and <c>Mechanic</c> entities,
    /// enabling the linkage of multiple mechanics to a single game and vice versa.
    /// </remarks>
    public virtual ICollection<GameMechanic> GameMechanics { get; set; } = [];
}