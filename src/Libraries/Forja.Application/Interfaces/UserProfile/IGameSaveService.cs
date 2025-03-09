namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Service interface for managing GameSave functionality.
/// </summary>
public interface IGameSaveService
{
    /// <summary>
    /// Retrieves all GameSaves.
    /// </summary>
    /// <returns>A collection of all GameSaves, including related User and UserLibraryGame entities.</returns>
    Task<List<GameSaveDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific GameSave by its ID.
    /// </summary>
    /// <param name="id">The ID of the GameSave to retrieve.</param>
    /// <returns>The requested GameSave if found, or null otherwise.</returns>
    Task<GameSaveDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a list of GameSaves based on the specified filters.
    /// </summary>
    /// <param name="libraryGameId">The ID of the library game to filter by, or null to ignore this filter.</param>
    /// <param name="userId">The ID of the user to filter by, or null to ignore this filter.</param>
    /// <returns>A list of GameSaves matching the provided filters.</returns>
    Task<List<GameSaveDto>> GetAllByFilterAsync(Guid? libraryGameId, Guid? userId);

    /// <summary>
    /// Adds a new GameSave.
    /// </summary>
    /// <param name="request">The GameSaveCreateRequest entity to add.</param>
    /// <returns>The added GameSave entity.</returns>
    Task<GameSaveDto?> AddAsync(GameSaveCreateRequest request);

    /// <summary>
    /// Updates an existing GameSave.
    /// </summary>
    /// <param name="request">The GameSaveUpdateRequest entity to update.</param>
    /// <returns>The updated GameSave entity.</returns>
    Task<GameSaveDto?> UpdateAsync(GameSaveUpdateRequest request);

    /// <summary>
    /// Deletes a GameSave by its ID.
    /// </summary>
    /// <param name="id">The ID of the GameSave to delete.</param>
    /// <returns>A task representing the delete operation.</returns>
    Task DeleteAsync(Guid id);
}