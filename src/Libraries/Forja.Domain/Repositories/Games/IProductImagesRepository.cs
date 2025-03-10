namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing the many-to-many relationship between products and item images.
/// </summary>
public interface IProductImagesRepository
{
    /// <summary>
    /// Gets all product-image relationships.
    /// </summary>
    /// <returns>A collection of all product-image relationships.</returns>
    Task<IEnumerable<ProductImages>> GetAllAsync();

    /// <summary>
    /// Gets a specific product-image relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-image relationship.</param>
    /// <returns>The product-image relationship with the specified ID, or null if not found.</returns>
    Task<ProductImages?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new product-image relationship to the repository.
    /// </summary>
    /// <param name="productImage">The product-image relationship to add.</param>
    /// <returns>The added product-image relationship.</returns>
    Task<ProductImages?> AddAsync(ProductImages productImage);

    /// <summary>
    /// Deletes a product-image relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-image relationship to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all product-image relationships including associated product and item image details.
    /// </summary>
    /// <returns>A collection of product-image relationships including related products and item images.</returns>
    Task<IEnumerable<ProductImages>> GetAllWithDetailsAsync();

    /// <summary>
    /// Finds product-image relationships by product ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product-image relationships associated with the product ID.</returns>
    Task<IEnumerable<ProductImages>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Finds product-image relationships by item image ID.
    /// </summary>
    /// <param name="itemImageId">The unique identifier of the item image.</param>
    /// <returns>A collection of product-image relationships associated with the item image ID.</returns>
    Task<IEnumerable<ProductImages>> GetByItemImageIdAsync(Guid itemImageId);
}