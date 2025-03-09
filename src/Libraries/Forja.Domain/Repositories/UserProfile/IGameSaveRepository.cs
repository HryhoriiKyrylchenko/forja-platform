namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Interface for the repository that manages the persistence and retrieval of GameSave entities.
/// </summary>
/// <remarks>
/// Provides methods to perform CRUD operations on GameSave data in a data store.
/// These operations include retrieving all game saves, retrieving a specific game save by ID, adding a new game save,
/// updating an existing game save, and deleting a game save by ID.
/// </remarks>
public interface IGameSaveRepository
{
    /// <summary>
    /// Asynchronously retrieves all game saves from the repository, including related user and user library game entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a collection of all game saves.</returns>
    Task<IEnumerable<GameSave>> GetAllAsync();

    /// <summary>
    /// Retrieves a GameSave entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the GameSave entity to be retrieved.</param>
    /// <returns>A Task representing the asynchronous operation. The Task result contains the GameSave entity if found, or null if not found.</returns>
    Task<GameSave?> GetByIdAsync(Guid id);


    /// <summary>
    /// Asynchronously retrieves a filtered list of game saves from the repository based on the provided criteria.
    /// </summary>
    /// <param name="libraryGameId">Optional identifier of the library game to filter the game saves.</param>
    /// <param name="userId">Optional identifier of the user to filter the game saves.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a list of game saves matching the provided filter criteria.</returns>
    Task<List<GameSave>> GetAllByFilterAsync(Guid? libraryGameId, Guid? userId);

    /// <summary>
    /// Adds a new game save entity to the data store.
    /// </summary>
    /// <param name="gameSave">The <see cref="GameSave"/> object to be added.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the added <see cref="GameSave"/> entity.</returns>
    Task<GameSave?> AddAsync(GameSave gameSave);

    /// <summary>
    /// Updates an existing game save entry in the data store.
    /// </summary>
    /// <param name="gameSave">The game save entity to update.</param>
    /// <returns>The updated game save entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided game save entity is null.</exception>
    Task<GameSave?> UpdateAsync(GameSave gameSave);

    /// <summary>
    /// Deletes a GameSave entity with the specified identifier from the data store.
    /// </summary>
    /// <param name="id">The unique identifier of the GameSave to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a GameSave with the specified identifier is not found.</exception>
    Task DeleteAsync(Guid id);
}