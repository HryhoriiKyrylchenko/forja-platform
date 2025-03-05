namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Service interface for UserFollower functionality.
/// Provides high-level operations for managing UserFollower entities.
/// </summary>
public interface IUserFollowerService
{
    /// <summary>
    /// Gets all UserFollower entries.
    /// </summary>
    /// <returns>A list of UserFollowerDTO containing all entries.</returns>
    Task<IEnumerable<UserFollowerDto>> GetAllAsync();

    /// <summary>
    /// Gets a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserFollower entry.</param>
    /// <returns>A UserFollowerDTO corresponding to the given ID, or null if not found.</returns>
    Task<UserFollowerDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new UserFollower entry.
    /// </summary>
    /// <param name="followerId">The unique identifier of the follower user.</param>
    /// <param name="followedId">The unique identifier of the followed user.</param>
    /// <returns>The created UserFollowerDTO.</returns>
    Task<UserFollowerDto> AddAsync(Guid followerId, Guid followedId);

    /// <summary>
    /// Updates a UserFollower entry.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to update.</param>
    /// <param name="followerId">The updated follower user ID.</param>
    /// <param name="followedId">The updated followed user ID.</param>
    Task UpdateAsync(Guid id, Guid followerId, Guid followedId);

    /// <summary>
    /// Deletes a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all UserFollower entries where the specified user is the follower.
    /// </summary>
    /// <param name="userId">The unique identifier of the follower user.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is the follower.</returns>
    Task<IEnumerable<UserFollowerDto>> GetFollowersByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets all UserFollower entries where the specified user is being followed.
    /// </summary>
    /// <param name="userId">The unique identifier of the followed user.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is being followed.</returns>
    Task<IEnumerable<UserFollowerDto>> GetFollowedByUserIdAsync(Guid userId);
}