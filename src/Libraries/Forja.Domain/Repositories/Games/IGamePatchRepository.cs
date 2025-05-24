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


    /// Retrieves a specific game patch based on the game ID, platform, and the patch name.
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="platform">The platform type of the game patch (e.g., Windows, Mac, Linux).</param>
    /// <param name="patchName">The name of the patch to retrieve.</param>
    /// <return>
    /// A task that represents the asynchronous operation. The task result contains the retrieved GamePatch object if found; otherwise, null.
    /// </return>
    Task<GamePatch?> GetByGameIdPlatformAndPatchNameAsync(Guid gameId, PlatformType platform, string patchName);

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


    /// Retrieves a game patch based on the specified game ID, platform, and version range.
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="platform">The platform type (e.g., Windows, Mac, Linux) for the game patch.</param>
    /// <param name="fromVersion">The starting version of the patch range.</param>
    /// <param name="toVersion">The ending version of the patch range.</param>
    /// <return>
    /// A task that represents the asynchronous operation. The task result contains a GamePatch object if a matching patch is found; otherwise, null.
    /// </return>
    Task<GamePatch?> GetByGameIdPlatformAndVersionsAsync(Guid gameId, PlatformType platform, string fromVersion, string toVersion);
}
