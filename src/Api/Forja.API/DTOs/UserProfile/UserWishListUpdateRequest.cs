namespace Forja.API.DTOs.UserProfile;

/// <summary>
/// Request model for updating an existing UserWishList entry.
/// </summary>
public class UserWishListUpdateRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The unique identifier for the product.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
}