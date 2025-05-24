namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Defines the service for managing game patches.
/// </summary>
public interface IGamePatchService
{
    /// <summary>
    /// Adds a new game patch to the system based on the provided request data.
    /// </summary>
    /// <param name="request">An object containing the data required to create a new game patch.</param>
    /// <returns>A <see cref="GamePatchDto"/> object representing the newly created game patch,
    /// or null if the operation fails.</returns>
    Task<GamePatchDto?> AddGamePatch(GamePatchCreateRequest request);

    /// <summary>
    /// Updates an existing game patch with the specified details.
    /// </summary>
    /// <param name="request">The request containing the details to update the game patch.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the updated game patch details
    /// as a <see cref="GamePatchDto"/>. Returns null if the game patch does not exist or could not be updated.
    /// </returns>
    Task<GamePatchDto?> UpdateGamePatch(GamePatchUpdateRequest request);


    /// <summary>
    /// Retrieves a game patch by the specified game ID, platform, and patch name.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="platform">The platform type for which the patch is designed.</param>
    /// <param name="name">The name of the game patch to retrieve.</param>
    /// <returns>
    /// A <see cref="GamePatchDto"/> object representing the game patch that matches the provided
    /// criteria, or null if no matching patch is found.
    /// </returns>
    Task<GamePatchDto?> GetGamePatchByGameIdPlatformAndName(Guid gameId, PlatformType platform, string name);
}