namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing bundles in the repository.
/// </summary>
public interface IBundleRepository
{
    /// <summary>
    /// Gets all bundles.
    /// </summary>
    /// <returns>A collection of all bundles.</returns>
    Task<IEnumerable<Bundle>> GetAllActiveAsync();

    /// <summary>
    /// Gets an active bundle by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle.</param>
    /// <returns>The bundle with the specified ID, or null if not found.</returns>
    Task<Bundle?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new bundle to the repository.
    /// </summary>
    /// <param name="bundle">The bundle to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Bundle?> AddAsync(Bundle bundle);

    /// <summary>
    /// Updates an existing bundle in the repository.
    /// </summary>
    /// <param name="bundle">The bundle with updated information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Bundle?> UpdateAsync(Bundle bundle);

    /// <summary>
    /// Deletes a bundle by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all active bundles.
    /// </summary>
    /// <returns>A task representing the active bundles.</returns>
    Task<IEnumerable<Bundle>> GetActiveBundlesAsync();
}