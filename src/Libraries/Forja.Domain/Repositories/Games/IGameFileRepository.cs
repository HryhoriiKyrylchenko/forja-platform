namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for GameFile repository, providing methods for managing game file data.
/// </summary>
public interface IGameFileRepository
{
    /// <summary>
    /// Gets all game files.
    /// </summary>
    /// <returns>A collection of all <see cref="GameFile"/> entities.</returns>
    Task<IEnumerable<GameFile>> GetAllAsync();

    /// <summary>
    /// Gets a game file by its ID.
    /// </summary>
    /// <param name="id">The ID of the game file.</param>
    /// <returns>The <see cref="GameFile"/> entity if found; otherwise, null.</returns>
    Task<GameFile?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a game file by its associated game version ID and file name.
    /// </summary>
    /// <param name="gameVersionId">The ID of the game version associated with the game file.</param>
    /// <param name="fileName">The name of the game file.</param>
    /// <returns>The <see cref="GameFile"/> entity if found; otherwise, null.</returns>
    Task<GameFile?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName);

    /// <summary>
    /// Gets all files for a specific game version.
    /// </summary>
    /// <param name="gameVersionId">The ID of the game version.</param>
    /// <returns>A collection of <see cref="GameFile"/> entities associated with the specified game version.</returns>
    Task<IEnumerable<GameFile>> GetByGameVersionIdAsync(Guid gameVersionId);

    /// <summary>
    /// Adds a new game file.
    /// </summary>
    /// <param name="gameFile">The game file to add.</param>
    /// <returns>The added <see cref="GameFile"/> entity.</returns>
    Task<GameFile?> AddAsync(GameFile gameFile);

    /// <summary>
    /// Updates an existing game file.
    /// </summary>
    /// <param name="gameFile">The game file to update.</param>
    /// <returns>The updated <see cref="GameFile"/> entity.</returns>
    Task<GameFile?> UpdateAsync(GameFile gameFile);

    /// <summary>
    /// Deletes a game file by its ID.
    /// </summary>
    /// <param name="id">The ID of the game file to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Finds a game file by its version and file name.
    /// </summary>
    /// <param name="version">The version of the game to search for.</param>
    /// <param name="fileName">The name of the file to search for.</param>
    /// <returns>The game file matching the specified version and file name, or null if no match is found.</returns>
    Task<GameFile?> FindByVersionAndNameAsync(string version, string fileName);
}