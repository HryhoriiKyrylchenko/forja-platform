namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for GameVersion repository, providing methods for managing game version data.
/// </summary>
public interface IGameVersionRepository
{
    /// <summary>
    /// Gets all game versions.
    /// </summary>
    /// <returns>A collection of all <see cref="GameVersion"/> entities.</returns>
    Task<IEnumerable<GameVersion>> GetAllAsync();

    /// <summary>
    /// Gets a game version by its ID.
    /// </summary>
    /// <param name="id">The ID of the game version.</param>
    /// <returns>The <see cref="GameVersion"/> entity if found; otherwise, null.</returns>
    Task<GameVersion?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a specific game version by the game ID and version.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The <see cref="GameVersion"/> entity if found; otherwise, null.</returns>
    Task<GameVersion?> GetByGameIdAndVersionAsync(Guid gameId, string version);

    /// <summary>
    /// Gets all versions of a specific game.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <returns>A collection of <see cref="GameVersion"/> entities associated with the specified game.</returns>
    Task<IEnumerable<GameVersion>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Adds a new game version.
    /// </summary>
    /// <param name="gameVersion">The game version to add.</param>
    /// <returns>The added <see cref="GameVersion"/> entity.</returns>
    Task<GameVersion?> AddAsync(GameVersion gameVersion);

    /// <summary>
    /// Updates an existing game version.
    /// </summary>
    /// <param name="gameVersion">The game version to update.</param>
    /// <returns>The updated <see cref="GameVersion"/> entity.</returns>
    Task<GameVersion?> UpdateAsync(GameVersion gameVersion);

    /// <summary>
    /// Deletes a game version by its ID.
    /// </summary>
    /// <param name="id">The ID of the game version to delete.</param>
    Task DeleteAsync(Guid id);
}