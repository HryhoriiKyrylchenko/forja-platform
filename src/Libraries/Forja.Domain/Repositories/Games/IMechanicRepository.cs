namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing mechanic entities in the repository.
/// </summary>
public interface IMechanicRepository
{
    /// <summary>
    /// Gets all mechanics.
    /// </summary>
    /// <returns>A collection of all mechanics.</returns>
    Task<IEnumerable<Mechanic>> GetAllAsync();

    /// <summary>
    /// Gets all deleted mechanics.
    /// </summary>
    /// <returns>A collection of all deleted mechanics.</returns>
    Task<IEnumerable<Mechanic>> GetAllDeletedAsync();

    /// <summary>
    /// Gets a mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mechanic.</param>
    /// <returns>The mechanic with the specified ID, or null if not found.</returns>
    Task<Mechanic?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new mechanic to the repository.
    /// </summary>
    /// <param name="mechanic">The mechanic to add.</param>
    /// <returns>The added mechanic.</returns>
    Task<Mechanic?> AddAsync(Mechanic mechanic);

    /// <summary>
    /// Updates an existing mechanic in the repository.
    /// </summary>
    /// <param name="mechanic">The mechanic with updated information.</param>
    /// <returns>The updated mechanic.</returns>
    Task<Mechanic?> UpdateAsync(Mechanic mechanic);

    /// <summary>
    /// Deletes a mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mechanic to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);
}