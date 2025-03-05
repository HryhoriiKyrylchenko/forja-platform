namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// Data transfer object for UserWishList entity.
/// Simplifies the data returned for UserWishList-related operations.
/// </summary>
public class UserWishListDto
{
    /// <summary>
    /// The unique identifier for the UserWishList entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique ID of the user associated with the wishlist entry.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The username of the user associated with the wishlist.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The unique ID of the product associated with the wishlist entry.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The name of the product associated with the wishlist.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
}
