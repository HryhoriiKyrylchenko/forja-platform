namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Defines a repository interface for managing achievements in the user profile domain.
/// Provides methods for retrieving, adding, updating, and deleting achievements.
/// </summary>
public interface IAchievementRepository
{
    /// <summary>
    /// Retrieves an achievement by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the achievement to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the achievement if found; otherwise, null.</returns>
    Task<Achievement?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all achievements associated with a specific game identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which achievements are to be retrieved.</param>
    /// <returns>An asynchronous operation that returns an enumerable collection of achievements associated with the specified game.</returns>
    Task<IEnumerable<Achievement>> GetAllByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves all deleted achievements associated with a specific game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game whose deleted achievements are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of deleted achievements associated with the specified game.
    /// </returns>
    Task<IEnumerable<Achievement>> GetAllDeletedByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves all achievements from the data source asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a collection of all achievements.</returns>
    Task<IEnumerable<Achievement>> GetAllAsync();

    /// <summary>
    /// Retrieves all achievements that have been marked as deleted.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of deleted achievements.</returns>
    Task<IEnumerable<Achievement>> GetAllDeletedAsync();

    /// <summary>
    /// Asynchronously adds a new achievement to the repository.
    /// </summary>
    /// <param name="achievement">The achievement entity to be added to the repository.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddAsync(Achievement achievement);

    /// <summary>
    /// Updates an existing achievement with new values.
    /// </summary>
    /// <param name="achievement">The achievement entity containing updated data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Achievement achievement);

    /// <summary>
    /// Deletes an achievement from the repository using the provided achievement ID.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid achievementId);

    /// <summary>
    /// Restores a previously deleted achievement by its unique identifier.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement to restore.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the restored achievement if successful.</returns>
    Task<Achievement> RestoreAsync(Guid achievementId); 
}