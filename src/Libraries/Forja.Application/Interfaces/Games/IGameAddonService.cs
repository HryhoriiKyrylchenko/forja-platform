namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Represents a service for managing game addons, providing methods for CRUD operations and retrieval based on game-specific criteria.
/// </summary>
public interface IGameAddonService
{
    /// <summary>
    /// Asynchronously retrieves all game addon entries.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="GameAddonDto"/> objects representing the game addons.</returns>
    Task<List<GameAddonDto>> GetAllAsync();
    
    /// <summary>
    /// Asynchronously retrieves a list of game addon entries specifically formatted for use in the catalog.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="GameAddonShortDto"/> objects representing the catalog-specific game addons.</returns>
    Task<List<GameAddonShortDto>> GetAllForCatalogAsync();

    /// <summary>
    /// Retrieves a game addon asynchronously based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game addon.</param>
    /// <returns>A <see cref="GameAddonDto"/> containing the addon details if found, or null if not found.</returns>
    Task<GameAddonDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new game addon and returns the created addon details.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="GameAddonCreateRequest"/> containing the details required to create a new game addon.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation. The task result contains an instance of
    /// <see cref="GameAddonDto"/> representing the created addon, or null if the creation fails.
    /// </returns>
    Task<GameAddonDto?> CreateAsync(GameAddonCreateRequest request);

    /// <summary>
    /// Updates an existing game add-on with the provided details.
    /// </summary>
    /// <param name="request">The request containing the updated information for a game add-on.</param>
    /// <returns>A <see cref="GameAddonDto"/> representing the updated game add-on, or null if the update fails.</returns>
    Task<GameAddonDto?> UpdateAsync(GameAddonUpdateRequest request);

    /// <summary>
    /// Deletes a game addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game addon to delete.</param>
    /// <exception cref="ArgumentException">Thrown if the supplied identifier is empty.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Retrieves a collection of game addons associated with a specific game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which the addons are requested.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="GameAddonShortDto"/> representing the game addons associated with the specified game ID.</returns>
    Task<List<GameAddonShortDto>> GetByGameIdAsync(Guid gameId);
}