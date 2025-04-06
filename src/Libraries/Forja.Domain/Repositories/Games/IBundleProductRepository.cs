namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing bundle products in the repository.
/// </summary>
public interface IBundleProductRepository
{
    /// <summary>
    /// Gets all bundle products.
    /// </summary>
    /// <returns>A collection of all bundle products.</returns>
    Task<IEnumerable<BundleProduct>> GetAllAsync();

    /// <summary>
    /// Gets a bundle product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle product.</param>
    /// <returns>The bundle product with the specified ID, or null if not found.</returns>
    Task<BundleProduct?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new bundle product to the repository.
    /// </summary>
    /// <param name="bundleProduct">The bundle product to add.</param>
    /// <returns>The added bundle product.</returns>
    Task<BundleProduct?> AddAsync(BundleProduct bundleProduct);

    /// <summary>
    /// Updates an existing bundle product in the repository.
    /// </summary>
    /// <param name="bundleProduct">The bundle product with updated information.</param>
    /// <returns>The updated bundle product.</returns>
    Task<BundleProduct?> UpdateAsync(BundleProduct bundleProduct);

    /// <summary>
    /// Deletes a bundle product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bundle product to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Gets all bundle products for a specific bundle.
    /// </summary>
    /// <param name="bundleId">The unique identifier of the bundle.</param>
    /// <returns>A collection of bundle products for the specified bundle.</returns>
    Task<IEnumerable<BundleProduct>> GetByBundleIdAsync(Guid bundleId);

    /// <summary>
    /// Distributes the total price of a bundle proportionally among its individual products.
    /// </summary>
    /// <param name="bundleProducts">A list of the products included in the bundle.</param>
    /// <param name="bundleTotalPrice">The total price of the bundle to be distributed.</param>
    /// <returns>A list of bundle products with their respective distributed prices updated.</returns>
    List<BundleProduct> DistributeBundlePrice(List<BundleProduct> bundleProducts, decimal bundleTotalPrice);
}