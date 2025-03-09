namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents the white list entry for a user and a specific game in the system.
/// </summary>
/// <remarks>
/// This class associates a user with a game in a whitelist context,
/// where the user has special privileges or access specific to that game.
/// It includes navigation properties to the corresponding user and game entities.
/// </remarks>
[Table("UserWishLists", Schema = "user-profile")]
public class UserWishList
{
    /// <summary>
    /// Gets or sets the unique identifier for the UserWhiteList entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents a foreign key reference to the associated <see cref="Product"/> entity in the whitelist.
    /// Used to link a user to a specific product in the context of the whitelist functionality.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the whitelist entry.
    /// This property establishes a foreign key relationship to the User entity,
    /// allowing access to the associated User object.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents a product in the system.
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public virtual User User { get; set; } = null!;
}