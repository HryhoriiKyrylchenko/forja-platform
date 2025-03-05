namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Service interface for UserWishList functionality.
/// Provides high-level operations for managing UserWishList entities.
/// </summary>
public interface IUserWishListService
{
    /// <summary>
    /// Retrieves all UserWishList entries.
    /// </summary>
    /// <returns>A list of UserWishListDTO containing all entries.</returns>
    Task<IEnumerable<UserWishListDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserWishList entry.</param>
    /// <returns>A UserWishListDTO corresponding to the given ID, or null if not found.</returns>
    Task<UserWishListDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new entry to the UserWishList for a specific user and product.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="productId">The unique identifier of the product to add to the wishlist.</param>
    /// <returns>The created UserWishListDTO.</returns>
    Task<UserWishListDto> AddAsync(Guid userId, Guid productId);

    /// <summary>
    /// Updates an existing UserWishList entry.
    /// </summary>
    /// <param name="id">The unique identifier of the UserWishList entry to update.</param>
    /// <param name="userId">The updated user ID.</param>
    /// <param name="productId">The updated product ID.</param>
    Task UpdateAsync(Guid id, Guid userId, Guid productId);

    /// <summary>
    /// Deletes a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserWishList entry to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Retrieves all UserWishList entries associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of UserWishListDTO entries specific to the user.</returns>
    Task<IEnumerable<UserWishListDto>> GetByUserIdAsync(Guid userId);
}