namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Defines the repository interface for managing UserLibraryAddon entities.
/// Provides methods for retrieving, adding, updating, and deleting UserLibraryAddon objects in the data storage.
/// </summary>
public interface IUserLibraryAddonRepository
{
    /// <summary>
    /// Retrieves a user library addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user library addon.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user library addon if found, or null if no addon with the specified identifier exists.</returns>
    Task<UserLibraryAddon?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a deleted user library addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the deleted user library addon.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted user library addon if found, or null if no deleted addon with the specified identifier exists.</returns>
    Task<UserLibraryAddon?> GetDeletedByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user library addon based on the specified game identifier and user identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user library addon if found, or null if no addon with the specified game and user identifiers exists.</returns>
    Task<UserLibraryAddon?> GetByGameIdAndUserIdAsync(Guid gameId, Guid userId);

    /// <summary>
    /// Retrieves all user library addons associated with a specific addon identifier.
    /// </summary>
    /// <param name="addonId">The unique identifier of the addon.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of user library addons associated with the specified addon identifier.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetByAddonIdAsync(Guid addonId);

    /// <summary>
    /// Asynchronously retrieves all user library addons associated with a specific game.
    /// </summary>
    /// <param name="gameId">The id of the game for which to retrieve associated library addons.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable collection of UserLibraryAddon objects associated with the specified game.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves all deleted user library addons associated with a specific game by its unique identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which deleted user library addons are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of deleted user library addons related to the specified game.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllDeletedByGameIdAsync(Guid gameId);

    /// <summary>
    /// Asynchronously retrieves all user library addons from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of UserLibraryAddon objects.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllAsync();

    /// <summary>
    /// Retrieves all deleted user library addons.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of deleted user library addons.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllDeletedAsync();

    /// <summary>
    /// Asynchronously adds a new UserLibraryAddon entity to the repository.
    /// </summary>
    /// <param name="userLibraryAddon">
    /// An instance of <see cref="UserLibraryAddon"/> representing the addon to add.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task<UserLibraryAddon?> AddAsync(UserLibraryAddon userLibraryAddon);

    /// <summary>
    /// Updates an existing UserLibraryAddon entity in the repository.
    /// </summary>
    /// <param name="userLibraryAddon">
    /// The UserLibraryAddon entity containing updated information to be saved.
    /// </param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// </returns>
    Task<UserLibraryAddon?> UpdateAsync(UserLibraryAddon userLibraryAddon);

    /// <summary>
    /// Deletes a user library addon from the repository asynchronously.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Restores a previously deleted user library addon to an active state.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to restore.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the restored user library addon.</returns>
    Task<UserLibraryAddon?> RestoreAsync(Guid userLibraryAddonId);
}