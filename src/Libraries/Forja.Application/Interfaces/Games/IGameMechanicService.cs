namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Defines a contract for managing game mechanics. Allows for operations such as
/// retrieving, creating, updating, and deleting game mechanic associations.
/// </summary>
public interface IGameMechanicService
{
    /// <summary>
    /// Retrieves all game mechanics.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="GameMechanicDto"/> objects
    /// representing all game mechanics in the system.</returns>
    Task<IEnumerable<GameMechanicDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves a game mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game mechanic to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The task result contains the <see cref="GameMechanicDto"/> representing the game mechanic if found, or <c>null</c> if no game mechanic with the specified identifier exists.</returns>
    Task<GameMechanicDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a collection of game mechanics associated with a specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which mechanics are being retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable collection of <see cref="GameMechanicDto"/> representing the game mechanics associated with the specified game.</returns>
    Task<IEnumerable<GameMechanicDto>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves a collection of game mechanics associated with a specific mechanic ID.
    /// </summary>
    /// <param name="mechanicId">The unique identifier of the mechanic to filter the game mechanics by.</param>
    /// <returns>An enumerable collection of <see cref="GameMechanicDto"/> objects representing the game mechanics associated with the specified mechanic ID.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided mechanic ID is invalid or empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no game mechanics are found for the provided mechanic ID.</exception>
    Task<IEnumerable<GameMechanicDto>> GetByMechanicIdAsync(Guid mechanicId);

    /// <summary>
    /// Creates a new association between a game and a game mechanic based on the provided request data.
    /// </summary>
    /// <param name="request">The request containing the game ID and mechanic ID to associate.</param>
    /// <returns>A <see cref="GameMechanicDto"/> representing the created association, or null if creation failed.</returns>
    Task<GameMechanicDto?> CreateAsync(GameMechanicCreateRequest request);

    /// <summary>
    /// Updates an existing game mechanic with the provided data.
    /// </summary>
    /// <param name="request">A <see cref="GameMechanicUpdateRequest"/> containing the updated information for the game mechanic.</param>
    /// <returns>
    /// A <see cref="GameMechanicDto"/> representing the updated game mechanic, or null if the update process fails.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the provided request is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the game mechanic with the specified ID does not exist.</exception>
    Task<GameMechanicDto?> UpdateAsync(GameMechanicUpdateRequest request);

    /// <summary>
    /// Deletes a GameMechanic entity identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the GameMechanic to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided ID is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no GameMechanic is found with the specified ID.</exception>
    Task DeleteAsync(Guid id);
}