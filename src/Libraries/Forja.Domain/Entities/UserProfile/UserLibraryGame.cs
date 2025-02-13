namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents a game in a user's library.
/// </summary>
[Table("UserLibraryGames", Schema = "user-profile")]
public class UserLibraryGame : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the user library game.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the game.
    /// </summary>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the game was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the user associated with this library game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the game associated with this library game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Game Game { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the collection of purchased addons for this library game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserLibraryAddon> PurchasedAddons { get; set; } = [];
}