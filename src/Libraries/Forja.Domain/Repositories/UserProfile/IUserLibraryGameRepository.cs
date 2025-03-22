namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Interface for managing UserLibraryGame entities in the repository.
/// </summary>
public interface IUserLibraryGameRepository
{
    /// <summary>
    /// Retrieves a user library game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user library game to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="UserLibraryGame"/> object if found; otherwise, null.
    /// </returns>
    Task<UserLibraryGame?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a soft-deleted user library game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the soft-deleted user library game to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="UserLibraryGame"/> object if found; otherwise, null.
    /// </returns>
    Task<UserLibraryGame?> GetDeletedByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user library addon by the associated game identifier and user identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game associated with the addon.</param>
    /// <param name="userId">The unique identifier of the user associated with the addon.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="UserLibraryGame"/> object if found; otherwise, null.
    /// </returns>
    Task<UserLibraryGame?> GetByGameIdAndUserIdAsync(Guid gameId, Guid userId);

    /// <summary>
    /// Retrieves all user library games associated with the specified game identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to search for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of
    /// <see cref="UserLibraryGame"/> objects associated with the specified game identifier.
    /// </returns>
    Task<IEnumerable<UserLibraryGame>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Asynchronously retrieves all user library games.
    /// </summary>
    /// <returns>A collection of <see cref="UserLibraryGame"/> entities.</returns>
    Task<IEnumerable<UserLibraryGame>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all soft-deleted user library games.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of
    /// <see cref="UserLibraryGame"/> entities that have been soft-deleted.
    /// </returns>
    Task<IEnumerable<UserLibraryGame>> GetAllDeletedAsync();

    /// <summary>
    /// Asynchronously retrieves all user library games associated with a specific user by their unique user identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose library games are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection
    /// of <see cref="UserLibraryGame"/> objects associated with the specified user.
    /// </returns>
    Task<IEnumerable<UserLibraryGame>> GetAllByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all soft-deleted user library games associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose soft-deleted games are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection
    /// of <see cref="UserLibraryGame"/> entities associated with the specified user that were soft-deleted.
    /// </returns>
    Task<IEnumerable<UserLibraryGame>> GetAllDeletedByUserIdAsync(Guid userId);

    /// <summary>
    /// Asynchronously adds a new UserLibraryGame entity to the repository.
    /// </summary>
    /// <param name="userLibraryGame">The UserLibraryGame instance to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<UserLibraryGame?> AddAsync(UserLibraryGame userLibraryGame);

    /// <summary>
    /// Updates an existing UserLibraryGame entity in the data store asynchronously.
    /// </summary>
    /// <param name="userLibraryGame">
    /// The UserLibraryGame entity to be updated, containing any modified data to be persisted.
    /// </param>
    /// <returns>
    /// A Task representing the asynchronous operation.
    /// </returns>
    Task<UserLibraryGame?> UpdateAsync(UserLibraryGame userLibraryGame);

    /// <summary>
    /// Deletes a UserLibraryGame entity from the repository by its identifier.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the UserLibraryGame to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid userLibraryGameId);

    /// <summary>
    /// Restores a previously soft-deleted user library game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user library game to restore.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the restored
    /// <see cref="UserLibraryGame"/> object if restoration is successful.
    /// </returns>
    Task<UserLibraryGame?> RestoreAsync(Guid id);
}