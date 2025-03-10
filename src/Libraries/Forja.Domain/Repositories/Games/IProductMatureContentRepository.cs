namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing the many-to-many relationship between products and mature content.
/// </summary>
public interface IProductMatureContentRepository
{
    /// <summary>
    /// Gets all product-mature content relationships.
    /// </summary>
    /// <returns>A collection of all product-mature content relationships.</returns>
    Task<IEnumerable<ProductMatureContent>> GetAllAsync();

    /// <summary>
    /// Gets a specific product-mature content relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-mature content relationship.</param>
    /// <returns>The product-mature content relationship with the specified ID, or null if not found.</returns>
    Task<ProductMatureContent?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new product-mature content relationship to the repository.
    /// </summary>
    /// <param name="productMatureContent">The product-mature content relationship to add.</param>
    /// <returns>The added product-mature content relationship.</returns>
    Task<ProductMatureContent?> AddAsync(ProductMatureContent productMatureContent);

    /// <summary>
    /// Deletes a product-mature content relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-mature content relationship to delete.</param>
    /// <returns>A task representing the completion of the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all product-mature content relationships, including associated product and mature content details.
    /// </summary>
    /// <returns>A collection of product-mature content relationships with their associated products and mature content.</returns>
    Task<IEnumerable<ProductMatureContent>> GetAllWithDetailsAsync();

    /// <summary>
    /// Finds product-mature content relationships by product ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product-mature content relationships associated with the given product ID.</returns>
    Task<IEnumerable<ProductMatureContent>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Finds product-mature content relationships by mature content ID.
    /// </summary>
    /// <param name="matureContentId">The unique identifier of the mature content.</param>
    /// <returns>A collection of product-mature content relationships associated with the given mature content ID.</returns>
    Task<IEnumerable<ProductMatureContent>> GetByMatureContentIdAsync(Guid matureContentId);
}