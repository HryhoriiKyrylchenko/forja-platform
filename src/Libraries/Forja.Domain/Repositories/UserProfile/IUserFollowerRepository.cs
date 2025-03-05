namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Repository interface for UserFollower entity.
/// Provides abstraction for UserFollower-specific data operations.
/// </summary>
public interface IUserFollowerRepository
{
    /// <summary>
    /// Gets all UserFollower entries from the database.
    /// </summary>
    /// <returns>A list of all UserFollower entries.</returns>
    Task<IEnumerable<UserFollower>> GetAllAsync();

    /// <summary>
    /// Gets a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserFollower entry.</param>
    /// <returns>The corresponding UserFollower entity, or null if not found.</returns>
    Task<UserFollower?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new UserFollower entry to the database.
    /// </summary>
    /// <param name="userFollower">The UserFollower entity to add.</param>
    /// <returns>The added UserFollower entity.</returns>
    Task<UserFollower> AddAsync(UserFollower userFollower);

    /// <summary>
    /// Updates an existing UserFollower entry.
    /// </summary>
    /// <param name="userFollower">The UserFollower entity with updated values.</param>
    Task UpdateAsync(UserFollower userFollower);

    /// <summary>
    /// Deletes a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserFollower entry to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all UserFollower entries where a specific user is the follower.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is the follower.</param>
    /// <returns>A list of UserFollower entries where the user is the follower.</returns>
    Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets all UserFollower entries where a specific user is being followed.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is being followed.</param>
    /// <returns>A list of UserFollower entries where the user is followed.</returns>
    Task<IEnumerable<UserFollower>> GetFollowedByUserIdAsync(Guid userId);
}