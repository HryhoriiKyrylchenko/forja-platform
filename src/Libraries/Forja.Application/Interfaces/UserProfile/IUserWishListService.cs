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
    Task<List<UserWishListDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserWishList entry.</param>
    /// <returns>A UserWishListDTO corresponding to the given ID, or null if not found.</returns>
    Task<UserWishListDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new entry to the UserWishList for a specific user and product.
    /// </summary>
    /// <param name="request">The UserWishListCreateRequest of the user.</param>
    /// <returns>The created UserWishListDTO.</returns>
    Task<UserWishListDto?> AddAsync(UserWishListCreateRequest request);

    /// <summary>
    /// Updates an existing UserWishList entry.
    /// </summary>
    /// <param name="request">The UserWishListUpdateRequest to update.</param>
    Task<UserWishListDto?> UpdateAsync(UserWishListUpdateRequest request);

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
    Task<List<UserWishListDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves the total number of items in a user's wish list asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier for the user whose wish list count is being requested.</param>
    /// <returns>An integer representing the count of items in the specified user's wish list.</returns>
    Task<int> GetWishListCountAsync(Guid userId);

    /// <summary>
    /// Retrieves UserWishList entries for a specific user along with extended game details.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose wish list entries are to be retrieved.</param>
    /// <returns>A list of UserWishListWithExtendedGameDto containing wish list entries and extended game details.</returns>
    Task<List<UserWishListWithExtendedGameDto>> GetByUserIdWithExtendedGameAsync(Guid userId);
}