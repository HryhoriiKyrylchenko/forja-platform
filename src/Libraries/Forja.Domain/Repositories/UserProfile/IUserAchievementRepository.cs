namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Provides abstraction for managing user achievement data in the system.
/// </summary>
/// <remarks>
/// This interface defines methods for handling operations such as retrieving, adding,
/// updating, and deleting user achievements. It acts as a contract for any repository
/// implementation managing user achievement data.
/// </remarks>
public interface IUserAchievementRepository
{
    /// <summary>
    /// Retrieves a user achievement by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user achievement to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user achievement if found, or null if not found.</returns>
    Task<UserAchievement?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all user achievements from the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an enumerable collection of <see cref="UserAchievement"/>.
    /// </returns>
    Task<IEnumerable<UserAchievement>> GetAllAsync();

    /// <summary>
    /// Adds a new user achievement to the repository asynchronously.
    /// </summary>
    /// <param name="userAchievement">The user achievement object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(UserAchievement userAchievement);

    /// <summary>
    /// Updates an existing user achievement in the repository.
    /// </summary>
    /// <param name="userAchievement">The user achievement entity to be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(UserAchievement userAchievement);

    /// <summary>
    /// Asynchronously deletes a user achievement by its unique identifier.
    /// </summary>
    /// <param name="userAchievementId">The unique identifier of the user achievement to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid userAchievementId);
}