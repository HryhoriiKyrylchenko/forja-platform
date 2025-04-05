namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Interface for managing bundle-product associations. Provides methods for
/// retrieving, creating, updating, and deleting bundle-product relationships.
/// </summary>
public interface IBundleProductService
{
    /// <summary>
    /// Retrieves all bundle product records asynchronously.
    /// </summary>
    /// <returns>An asynchronous operation that returns a collection of bundle product data transfer objects.</returns>
    Task<List<BundleProductDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves a bundle product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle product to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the
    /// corresponding <see cref="BundleProductDto"/> if found; otherwise, null.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no bundle product with the specified identifier is found.
    /// </exception>
    Task<BundleProductDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all bundle products associated with a specified bundle ID.
    /// </summary>
    /// <param name="bundleId">The unique identifier of the bundle for which the bundle products will be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="BundleProductDto"/> objects associated with the specified bundle ID.</returns>
    Task<List<BundleProductDto>> GetByBundleIdAsync(Guid bundleId);

    /// <summary>
    /// Creates a new bundle product association based on the provided request object.
    /// </summary>
    /// <param name="request">
    /// An object of type <see cref="BundleProductCreateRequest"/> containing details
    /// of the bundle product association to be created, including BundleId and ProductId.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, which upon completion contains
    /// an object of type <see cref="BundleProductDto"/> representing the created bundle product.
    /// Returns null if the creation process fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="request"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an error occurs during the creation process.
    /// </exception>
    Task<BundleProductDto?> CreateAsync(BundleProductCreateRequest request);

    /// <summary>
    /// Updates an existing bundle-product relationship.
    /// </summary>
    /// <param name="request">The request object containing details for the bundle-product update, including its ID, associated bundle ID, and product ID.</param>
    /// <returns>A <see cref="BundleProductDto"/> representing the updated bundle-product relationship, or null if the update fails.</returns>
    Task<BundleProductDto?> UpdateAsync(BundleProductUpdateRequest request);

    /// <summary>
    /// Deletes a bundle product entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle product to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);
}