namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents an addon for a game, inheriting from the Product class.
/// </summary>
[Table("GameAddons", Schema = "games")]
public class GameAddon : Product
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated game.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the associated game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the collection of user library addons associated with this game addon.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserLibraryAddon> UserLibraryAddons { get; set; } = [];
}