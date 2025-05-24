namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing game mechanic entities in the repository.
/// </summary>
public interface IGameMechanicRepository
{
    /// <summary>
    /// Gets all game mechanics.
    /// </summary>
    /// <returns>A collection of all game mechanics.</returns>
    Task<IEnumerable<GameMechanic>> GetAllAsync();

    /// <summary>
    /// Gets a game mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game mechanic.</param>
    /// <returns>The game mechanic with the specified ID, or null if not found.</returns>
    Task<GameMechanic?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all game mechanics associated with a specific game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>A collection of game mechanics associated with the specified game.</returns>
    Task<IEnumerable<GameMechanic>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Gets all game mechanics for a specific mechanic (across games).
    /// </summary>
    /// <param name="mechanicId">The unique identifier of the mechanic.</param>
    /// <returns>A collection of game mechanics associated with the specified mechanic.</returns>
    Task<IEnumerable<GameMechanic>> GetByMechanicIdAsync(Guid mechanicId);

    /// <summary>
    /// Adds a new game mechanic to the repository.
    /// </summary>
    /// <param name="gameMechanic">The game mechanic to add.</param>
    /// <returns>The added game mechanic.</returns>
    Task<GameMechanic?> AddAsync(GameMechanic gameMechanic);

    /// <summary>
    /// Updates an existing game mechanic in the repository.
    /// </summary>
    /// <param name="gameMechanic">The game mechanic with updated information.</param>
    /// <returns>The updated game mechanic.</returns>
    Task<GameMechanic?> UpdateAsync(GameMechanic gameMechanic);

    /// <summary>
    /// Deletes a game mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game mechanic to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);
}