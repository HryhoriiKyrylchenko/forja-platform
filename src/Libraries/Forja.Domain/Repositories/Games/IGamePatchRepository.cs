namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Provides an abstraction for repository operations related to game patches.
/// </summary>
public interface IGamePatchRepository
{
    /// Retrieves all game patches.
    /// <return>
    /// A task that represents asynchronous operation. The task result contains an IEnumerable of GamePatch representing all game patches.
    /// </return>
    Task<IEnumerable<GamePatch>> GetAllAsync();

    /// <summary>
    /// Retrieves a game patch by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game patch to retrieve.</param>
    /// <returns>
    /// A <see cref="GamePatch"/> object if a match is found; otherwise, null.
    /// </returns>
    Task<GamePatch?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a specific game patch by the provided game ID and patch name.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to which the patch belongs.</param>
    /// <param name="patchName">The name of the patch to be retrieved.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <see cref="GamePatch"/> entity
    /// if it exists; otherwise, returns null.
    /// </returns>
    Task<GamePatch?> GetByGameIdAndPatchNameAsync(Guid gameId, string patchName);

    /// <summary>
    /// Retrieves all game patches associated with a specific game based on the provided game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which to retrieve patches.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="GamePatch"/> associated with the specified game.</returns>
    Task<IEnumerable<GamePatch>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Adds a new game patch to the repository.
    /// </summary>
    /// <param name="gamePatch">The game patch instance to be added.</param>
    /// <returns>A <see cref="GamePatch"/> instance representing the added patch, or null if the operation fails.</returns>
    Task<GamePatch?> AddAsync(GamePatch gamePatch);

    /// <summary>
    /// Updates an existing game patch in the repository with new data.
    /// </summary>
    /// <param name="gamePatch">The game patch entity to be updated, containing the new data.</param>
    /// <returns>
    /// Returns the updated <see cref="GamePatch"/> entity if the update is successful,
    /// or null if the game patch does not exist in the repository.
    /// </returns>
    Task<GamePatch?> UpdateAsync(GamePatch gamePatch);

    /// <summary>
    /// Deletes a game patch based on the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game patch to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Retrieves a game patch based on the specified game ID and version range.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="fromVersion">The starting version of the patch.</param>
    /// <param name="toVersion">The target version of the patch.</param>
    /// <return>
    /// A task that represents the asynchronous operation. The task result contains a GamePatch object representing the specified patch, or null if not found.
    /// </return>
    Task<GamePatch?> GetByGameIdAndVersionsAsync(Guid gameId, string fromVersion, string toVersion);
}
