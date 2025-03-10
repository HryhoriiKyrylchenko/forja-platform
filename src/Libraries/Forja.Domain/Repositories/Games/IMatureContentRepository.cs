namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing mature content entities in the repository.
/// </summary>
public interface IMatureContentRepository
{
    /// <summary>
    /// Gets all mature content entries.
    /// </summary>
    /// <returns>A collection of all mature content entries.</returns>
    Task<IEnumerable<MatureContent>> GetAllAsync();

    /// <summary>
    /// Gets a mature content entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mature content.</param>
    /// <returns>The mature content entry with the specified ID, or null if not found.</returns>
    Task<MatureContent?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new mature content entry to the repository.
    /// </summary>
    /// <param name="matureContent">The mature content entry to add.</param>
    /// <returns>The added mature content entry.</returns>
    Task<MatureContent?> AddAsync(MatureContent matureContent);

    /// <summary>
    /// Updates an existing mature content entry in the repository.
    /// </summary>
    /// <param name="matureContent">The mature content with updated information.</param>
    /// <returns>The updated mature content entry.</returns>
    Task<MatureContent?> UpdateAsync(MatureContent matureContent);

    /// <summary>
    /// Deletes a mature content entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mature content to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all mature content entries with their associated product-mature content relationships.
    /// </summary>
    /// <returns>A collection of mature content entries including their related products.</returns>
    Task<IEnumerable<MatureContent>> GetAllWithProductMatureContentsAsync();
}