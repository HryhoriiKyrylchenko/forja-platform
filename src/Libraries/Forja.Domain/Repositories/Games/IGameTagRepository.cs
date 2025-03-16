namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing game tag entities in the repository.
/// </summary>
public interface IGameTagRepository
{
    /// <summary>
    /// Gets all game tags.
    /// </summary>
    /// <returns>A collection of all game tags.</returns>
    Task<IEnumerable<GameTag>> GetAllAsync();

    /// <summary>
    /// Gets a game tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game tag.</param>
    /// <returns>The game tag with the specified ID, or null if not found.</returns>
    Task<GameTag?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all game tags associated with a specific game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>A collection of game tags associated with the specified game.</returns>
    Task<IEnumerable<GameTag>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Gets all game tags associated with a specific tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>A collection of game tags associated with the specified tag.</returns>
    Task<IEnumerable<GameTag>> GetByTagIdAsync(Guid tagId);

    /// <summary>
    /// Adds a new game tag to the repository.
    /// </summary>
    /// <param name="gameTag">The game tag to add.</param>
    /// <returns>The added game tag.</returns>
    Task<GameTag?> AddAsync(GameTag gameTag);

    /// <summary>
    /// Updates an existing game tag in the repository.
    /// </summary>
    /// <param name="gameTag">The game tag with updated information.</param>
    /// <returns>The updated game tag.</returns>
    Task<GameTag?> UpdateAsync(GameTag gameTag);

    /// <summary>
    /// Deletes a game tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game tag to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);
}