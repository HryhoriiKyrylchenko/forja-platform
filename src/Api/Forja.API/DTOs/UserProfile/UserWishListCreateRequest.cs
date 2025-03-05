namespace Forja.API.DTOs.UserProfile;

/// <summary>
/// Request model for creating a UserWishList entry.
/// </summary>
public class UserWishListCreateRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The unique identifier for the product to add to the wishlist.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
}