namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents an addon purchased by a user for a specific game in their library.
/// </summary>
[Table("UserLibraryAddons", Schema = "user-profile")]
public class UserLibraryAddon : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the UserLibraryAddon.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the associated UserLibraryGame.
    /// </summary>
    [ForeignKey("UserLibraryGame")]
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated GameAddon.
    /// </summary>
    [ForeignKey("GameAddon")]
    public Guid AddonId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the addon was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the associated UserLibraryGame.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual UserLibraryGame UserLibraryGame { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the associated GameAddon.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual GameAddon GameAddon { get; set; } = null!;
}