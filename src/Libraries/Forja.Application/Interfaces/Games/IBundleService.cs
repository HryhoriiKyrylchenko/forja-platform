namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Defines the contract for operations related to managing game bundles.
/// </summary>
public interface IBundleService
{
    /// <summary>
    /// Asynchronously retrieves all game bundles.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing an enumerable of <see cref="BundleDto"/> objects.</returns>
    Task<IEnumerable<BundleDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all active game bundles.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an enumerable collection of active bundles as <see cref="BundleDto"/>.</returns>
    Task<IEnumerable<BundleDto>> GetActiveBundlesAsync();

    /// <summary>
    /// Retrieves a bundle associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="BundleDto"/> if the bundle exists, or null if the bundle is not found.
    /// </returns>
    Task<BundleDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously creates a new game bundle using the provided request data.
    /// </summary>
    /// <param name="request">An instance of <see cref="BundleCreateRequest"/> containing the details of the game bundle to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an instance of <see cref="BundleDto"/> representing the created game bundle, or null if creation was unsuccessful.</returns>
    Task<BundleDto?> CreateAsync(BundleCreateRequest request);

    /// <summary>
    /// Updates an existing game bundle with the provided details.
    /// </summary>
    /// <param name="request">An instance of <see cref="BundleUpdateRequest"/> containing the updated details of the bundle.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="BundleDto"/>, or null if the bundle was not found.</returns>
    Task<BundleDto?> UpdateAsync(BundleUpdateRequest request);

    /// <summary>
    /// Deletes a bundle with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle to delete.</param>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="id"/> is empty.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}