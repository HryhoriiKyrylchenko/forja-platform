namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides an interface for managing mature content associated with products.
/// </summary>
public interface IProductMatureContentService
{
    /// <summary>
    /// Asynchronously retrieves all ProductMatureContent records.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of <see cref="ProductMatureContentDto"/> objects representing the mature content information associated with products.
    /// </returns>
    Task<IEnumerable<ProductMatureContentDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a mature content entity associated with a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mature content entity to retrieve.</param>
    /// <returns>
    /// A <see cref="Task"/> that resolves to a <see cref="ProductMatureContentDto"/> if found, or null if no entity with the specified identifier exists.
    /// </returns>
    Task<ProductMatureContentDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a collection of mature content information associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which mature content information is being retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable collection of <see cref="ProductMatureContentDto"/> associated with the specified product.
    /// </returns>
    Task<IEnumerable<ProductMatureContentDto>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Retrieves a collection of ProductMatureContentDto objects associated with the specified mature content identifier.
    /// </summary>
    /// <param name="matureContentId">The unique identifier of the mature content for which associated product mature contents are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of ProductMatureContentDto objects.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no ProductMatureContents are found for the specified MatureContent ID.</exception>
    Task<IEnumerable<ProductMatureContentDto>> GetByMatureContentIdAsync(Guid matureContentId);

    /// <summary>
    /// Creates a new association between a product and mature content.
    /// </summary>
    /// <param name="request">The request object containing details about the product and mature content to be associated.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="ProductMatureContentDto"/> representing the created association, or null if the creation fails.
    /// </returns>
    Task<ProductMatureContentDto?> CreateAsync(ProductMatureContentCreateRequest request);

    /// <summary>
    /// Updates the mature content information associated with a product.
    /// </summary>
    /// <param name="request">The request containing the updated mature content details.</param>
    /// <returns>
    /// A <see cref="ProductMatureContentDto"/> representing the updated mature content information,
    /// or null if the update operation fails.
    /// </returns>
    Task<ProductMatureContentDto?> UpdateAsync(ProductMatureContentUpdateRequest request);

    /// <summary>
    /// Asynchronously deletes a ProductMatureContent entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ProductMatureContent to be deleted.</param>
    /// <exception cref="ArgumentException">Thrown when the provided ID is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no ProductMatureContent is found with the given ID.</exception>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}