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
    /// Retrieves a game patch by the associated game ID and patch name.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="name">The name of the patch to retrieve.</param>
    /// <returns>A <c>GamePatchDto</c> object representing the game patch if found, otherwise <c>null</c>.</returns>
    Task<GamePatchDto?> GetGamePatchByGameIdAndName(Guid gameId, string name);
}