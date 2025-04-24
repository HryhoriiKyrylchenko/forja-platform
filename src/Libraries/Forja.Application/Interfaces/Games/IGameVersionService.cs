namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Service interface for managing game versions.
/// </summary>
public interface IGameVersionService
{
    /// <summary>
    /// Adds a new game version to the repository.
    /// </summary>
    /// <param name="request">The request object containing details of the game version to be added.</param>
    /// <returns>
    /// A <see cref="GameVersionDto"/> object representing the added game version, or null if the addition failed.
    /// </returns>
    Task<GameVersionDto?> AddGameVersionAsync(GameVersionCreateRequest request);

    /// <summary>
    /// Updates an existing game version with new details.
    /// </summary>
    /// <param name="request">The update request containing the game version ID and updated details such as changelog, release date, etc.</param>
    /// <returns>A <see cref="GameVersionDto"/> representing the updated game version, or null if the operation was unsuccessful.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided request is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a game version with the specified ID is not found.</exception>
    Task<GameVersionDto?> UpdateGameVersionAsync(GameVersionUpdateRequest request);

    /// <summary>
    /// Retrieves a game version based on the specified game ID and version.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="version">The version string of the game to retrieve.</param>
    /// <returns>A <see cref="GameVersionDto"/> representing the game version if found; otherwise, null.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="gameId"/> is an empty GUID or <paramref name="version"/> is null, empty, or whitespace.</exception>
    Task<GameVersionDto?> GetGameVersionByGameIdAndVersionAsync(Guid gameId, string version);
}