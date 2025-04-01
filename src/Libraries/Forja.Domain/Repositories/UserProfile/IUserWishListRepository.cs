namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Repository interface for UserWishList entity.
/// Provides abstraction for UserWishList-specific data operations.
/// </summary>
public interface IUserWishListRepository
{
    /// <summary>
    /// Gets all UserWishList entries from the database.
    /// </summary>
    /// <returns>A list of all UserWishList entries.</returns>
    Task<IEnumerable<UserWishList>> GetAllAsync();

    /// <summary>
    /// Gets a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserWishList entry.</param>
    /// <returns>The corresponding UserWishList entity, or null if not found.</returns>
    Task<UserWishList?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new UserWishList entry to the database.
    /// </summary>
    /// <param name="userWishList">The UserWishList entity to add.</param>
    /// <returns>The added UserWishList entity.</returns>
    Task<UserWishList?> AddAsync(UserWishList userWishList);

    /// <summary>
    /// Updates an existing UserWishList entry.
    /// </summary>
    /// <param name="userWishList">The UserWishList entity with updated values.</param>
    Task<UserWishList?> UpdateAsync(UserWishList userWishList);

    /// <summary>
    /// Deletes a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserWishList entry to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all UserWishList entries for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of UserWishList entries associated with the user.</returns>
    Task<IEnumerable<UserWishList>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Asynchronously retrieves the count associated with a specific user identified by a unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier for the user whose count is being retrieved.</param>
    /// <returns>An integer representing the count related to the specified user.</returns>
    Task<int> GetCountByUserIdAsync(Guid userId);
}