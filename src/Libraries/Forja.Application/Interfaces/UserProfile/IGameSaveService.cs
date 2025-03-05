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
    Task<IEnumerable<GameSaveDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific GameSave by its ID.
    /// </summary>
    /// <param name="id">The ID of the GameSave to retrieve.</param>
    /// <returns>The requested GameSave if found, or null otherwise.</returns>
    Task<GameSaveDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new GameSave.
    /// </summary>
    /// <param name="gameSave">The GameSave entity to add.</param>
    /// <returns>The added GameSave entity.</returns>
    Task<GameSave> AddAsync(GameSaveDto gameSave);

    /// <summary>
    /// Updates an existing GameSave.
    /// </summary>
    /// <param name="gameSave">The GameSave entity to update.</param>
    /// <returns>The updated GameSave entity.</returns>
    Task<GameSave> UpdateAsync(GameSaveDto gameSave);

    /// <summary>
    /// Deletes a GameSave by its ID.
    /// </summary>
    /// <param name="id">The ID of the GameSave to delete.</param>
    /// <returns>A task representing the delete operation.</returns>
    Task DeleteAsync(Guid id);
}