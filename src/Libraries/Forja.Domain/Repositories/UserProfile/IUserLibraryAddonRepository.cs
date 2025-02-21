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
    /// Asynchronously retrieves all user library addons associated with a specific game.
    /// </summary>
    /// <param name="gameTitle">The title of the game for which to retrieve associated library addons.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable collection of UserLibraryAddon objects associated with the specified game.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllByGameAsync(string gameTitle);

    /// <summary>
    /// Asynchronously retrieves all user library addons from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of UserLibraryAddon objects.</returns>
    Task<IEnumerable<UserLibraryAddon>> GetAllAsync();

    /// <summary>
    /// Asynchronously adds a new UserLibraryAddon entity to the repository.
    /// </summary>
    /// <param name="userLibraryAddon">
    /// An instance of <see cref="UserLibraryAddon"/> representing the addon to add.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task AddAsync(UserLibraryAddon userLibraryAddon);

    /// <summary>
    /// Updates an existing UserLibraryAddon entity in the repository.
    /// </summary>
    /// <param name="userLibraryAddon">
    /// The UserLibraryAddon entity containing updated information to be saved.
    /// </param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// </returns>
    Task UpdateAsync(UserLibraryAddon userLibraryAddon);

    /// <summary>
    /// Deletes a user library addon from the repository asynchronously.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userLibraryAddonId);
}