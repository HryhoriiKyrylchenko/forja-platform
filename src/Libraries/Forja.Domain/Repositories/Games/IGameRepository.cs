namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing game entities in the repository.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Gets all games.
    /// </summary>
    /// <returns>A collection of all games.</returns>
    Task<IEnumerable<Game>> GetAllAsync();

    /// <summary>
    /// Gets a game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game.</param>
    /// <returns>The game with the specified ID, or null if not found.</returns>
    Task<Game?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new game to the repository.
    /// </summary>
    /// <param name="game">The game to add.</param>
    /// <returns>The added game.</returns>
    Task<Game?> AddAsync(Game game);

    /// <summary>
    /// Updates an existing game in the repository.
    /// </summary>
    /// <param name="game">The game with updated information.</param>
    /// <returns>The updated game.</returns>
    Task<Game?> UpdateAsync(Game game);

    /// <summary>
    /// Deletes a game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all games with a specific tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>A collection of games with the specified tag.</returns>
    Task<IEnumerable<Game>> GetByTagAsync(Guid tagId);
}

