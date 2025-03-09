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
    Task<List<UserFollowerDto>> GetAllAsync();

    /// <summary>
    /// Gets a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserFollower entry.</param>
    /// <returns>A UserFollowerDTO corresponding to the given ID, or null if not found.</returns>
    Task<UserFollowerDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Adds a new UserFollower entry.
    /// </summary>
    /// <param name="request">The request object containing the FollowerId and FollowedId for the new UserFollower entry.</param>
    /// <returns>The unique identifier of the newly created UserFollower entry.</returns>
    Task<UserFollowerDto?> AddAsync(UserFollowerCreateRequest request);

    /// <summary>
    /// Updates an existing UserFollower entry with the provided details.
    /// </summary>
    /// <param name="request">The UserFollowerUpdateRequest containing updated follower and followed user details.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task UpdateAsync(UserFollowerUpdateRequest request);

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
    Task<List<UserFollowerDto>> GetFollowersByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets all UserFollower entries where the specified user is being followed.
    /// </summary>
    /// <param name="userId">The unique identifier of the followed user.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is being followed.</returns>
    Task<List<UserFollowerDto>> GetFollowedByUserIdAsync(Guid userId);
}