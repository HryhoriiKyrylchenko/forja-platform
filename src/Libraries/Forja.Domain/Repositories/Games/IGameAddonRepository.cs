namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing game addon entities in the repository.
/// </summary>
public interface IGameAddonRepository
{
    /// <summary>
    /// Gets all game addons.
    /// </summary>
    /// <returns>A collection of all game addons.</returns>
    Task<IEnumerable<GameAddon>> GetAllAsync();

    /// <summary>
    /// Gets all game addons that have been marked as deleted.
    /// </summary>
    /// <returns>A collection of deleted game addons.</returns>
    Task<IEnumerable<GameAddon>> GetAllDeletedAsync();

    /// <summary>
    /// Gets a game addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game addon.</param>
    /// <returns>The game addon with the specified ID, or null if not found.</returns>
    Task<GameAddon?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Retrieves a game addon by its storage URL.
    /// </summary>
    /// <param name="storageUrl">The storage URL of the game addon.</param>
    /// <returns>The game addon associated with the specified storage URL, or null if not found.</returns>
    Task<GameAddon?> GetByStorageUrlAsync(string storageUrl);

    /// <summary>
    /// Gets all addons associated with a specific game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the associated game.</param>
    /// <returns>A collection of addons for the specified game.</returns>
    Task<IEnumerable<GameAddon>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Adds a new game addon to the repository.
    /// </summary>
    /// <param name="gameAddon">The game addon to add.</param>
    /// <returns>The added game addon.</returns>
    Task<GameAddon?> AddAsync(GameAddon gameAddon);

    /// <summary>
    /// Updates an existing game addon in the repository.
    /// </summary>
    /// <param name="gameAddon">The game addon with updated information.</param>
    /// <returns>The updated game addon.</returns>
    Task<GameAddon?> UpdateAsync(GameAddon gameAddon);

    /// <summary>
    /// Deletes a game addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game addon to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    
}