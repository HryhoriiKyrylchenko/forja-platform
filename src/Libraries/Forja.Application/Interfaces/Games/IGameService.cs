namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Defines the contract for game-related operations within the application.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Asynchronously retrieves all games as a collection of <see cref="GameDto"/>.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation that, when completed, contains
    /// an enumerable collection of <see cref="GameDto"/> representing the games.
    /// </returns>
    Task<IEnumerable<GameDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all games formatted for catalog view as a collection of <see cref="GameCatalogDto"/>.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation that, when completed, contains
    /// an enumerable collection of <see cref="GameCatalogDto"/> representing the games in catalog format.
    /// </returns>
    Task<List<GameCatalogDto>> GetAllForCatalogAsync();

    /// <summary>
    /// Retrieves a collection of all deleted game records asynchronously.
    /// </summary>
    /// <returns>An enumerable collection of GameDto objects representing the deleted games.</returns>
    Task<IEnumerable<GameDto>> GetAllDeletedAsync();

    /// <summary>
    /// Retrieves a game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game.</param>
    /// <returns>
    /// A <see cref="GameDto"/> representing the game if found; otherwise, null.
    /// </returns>
    Task<GameDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves detailed information about a game identified by its unique identifier
    /// as a <see cref="GameExtendedDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the game to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation that, when completed, contains
    /// a <see cref="GameExtendedDto"/> instance with detailed information about the specified game,
    /// or null if no game with the given identifier is found.
    /// </returns>
    Task<GameExtendedDto?> GetExtendedByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously adds a new game using the provided creation request.
    /// </summary>
    /// <param name="request">The request object containing the data for creating a new game.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created game as a <see cref="GameDto"/>, or null if the operation fails.</returns>
    Task<GameDto?> AddAsync(GameCreateRequest request);

    /// <summary>
    /// Updates an existing game record based on the provided game update request.
    /// </summary>
    /// <param name="request">An instance of <see cref="GameUpdateRequest"/> containing updated information for the game.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing an instance of <see cref="GameDto"/> for the updated game,
    /// or null if the update operation fails or the game is not found.
    /// </returns>
    Task<GameDto?> UpdateAsync(GameUpdateRequest request);

    /// <summary>
    /// Deletes a game permanently based on the provided identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game to be deleted. Must not be empty.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a collection of games associated with the specified tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag to filter the games by.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="GameDto"/> objects representing the games associated with the provided tag.</returns>
    Task<IEnumerable<GameDto>> GetByTagAsync(Guid tagId);
}