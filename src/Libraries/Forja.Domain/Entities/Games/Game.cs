namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a game entity with various properties and relationships.
/// </summary>
[Table("Games", Schema = "games")]
public class Game : Product
{
    /// <summary>
    /// Gets or sets the system requirements for the game.
    /// </summary>
    public string? SystemRequirements { get; set; }
    
    /// <summary>
    /// Gets or sets the storage URL for the game.
    /// </summary>
    public string? StorageUrl { get; set; }

    /// <summary>
    /// Gets or sets the total time played for the game.
    /// </summary>
    public TimeSpan? TimePlayed { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of game addons associated with the game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<GameAddon> GameAddons { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of game tags associated with the game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<GameTag> GameTags { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of user library games associated with the game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of reviews associated with the game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Review> Reviews { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of game mechanics associated with the game.
    /// </summary>
    public virtual ICollection<GameMechanic> GameMechanics { get; set; } = [];
}