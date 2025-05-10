namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Interface for managing game files in the application.
/// </summary>
public interface IGameFileService
{
    /// <summary>
    /// Adds a new game file to the repository.
    /// </summary>
    /// <param name="request">The request object containing the details of the game file to be added.</param>
    /// <returns>
    /// A <see cref="ProductFileDto"/> object containing the details of the added game file,
    /// or null if the operation fails.
    /// </returns>
    Task<ProductFileDto?> AddGameFile(GameFileCreateRequest request);

    /// <summary>
    /// Updates an existing game file with new data provided in the update request.
    /// </summary>
    /// <param name="request">The request object containing updated information for the game file.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// If the operation is successful, returns a <see cref="ProductFileDto"/> containing updated file details; otherwise, returns null.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the request violates business validation rules.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the game file specified by the ID does not exist.
    /// </exception>
    Task<ProductFileDto?> UpdateGameFile(GameFileUpdateRequest request);

    /// <summary>
    /// Retrieves a game file by its associated game version ID and file name.
    /// </summary>
    /// <param name="gameVersionId">The ID of the game version associated with the game file.</param>
    /// <param name="fileName">The name of the game file to retrieve.</param>
    /// <returns>A <see cref="ProductFileDto"/> object if the game file is found; otherwise, null.</returns>
    Task<ProductFileDto?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName);
}