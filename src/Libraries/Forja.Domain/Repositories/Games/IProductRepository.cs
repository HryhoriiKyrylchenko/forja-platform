namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing operations related to products.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets all products (including derived types).
    /// </summary>
    /// <returns>A collection of all products.</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product with the specified ID, or null if not found.</returns>
    Task<Product?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all products of a specific type.
    /// </summary>
    /// <typeparam name="T">The specific type of product (e.g., Game, GameAddon).</typeparam>
    /// <returns>A collection of products of the specified type.</returns>
    Task<IEnumerable<T>> GetByTypeAsync<T>() where T : Product;

    /// <summary>
    /// Retrieves a list of products based on their unique identifiers.
    /// </summary>
    /// <param name="productIds">A list of unique identifiers for the products to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of products corresponding to the specified identifiers.</returns>
    Task<List<Product>> GetProductsByIdsAsync(List<Guid> productIds);
}
