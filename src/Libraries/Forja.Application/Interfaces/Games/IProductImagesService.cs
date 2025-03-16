namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Defines the contract for managing product images in the application.
/// </summary>
public interface IProductImagesService
{
    /// <summary>
    /// Retrieves all product image records asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing an asynchronous operation that returns an enumerable collection of ProductImagesDto objects.
    /// </returns>
    Task<IEnumerable<ProductImagesDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a product image by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the product image.</param>
    /// <returns>A <see cref="ProductImagesDto"/> instance if found, otherwise null.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided ID is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no product image is found with the provided ID.</exception>
    Task<ProductImagesDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all product images including their detailed information.
    /// </summary>
    /// <returns>A collection of <see cref="ProductImagesDto"/> containing product image details.</returns>
    Task<IEnumerable<ProductImagesDto>> GetAllWithDetailsAsync();

    /// <summary>
    /// Retrieves a collection of product images associated with a specific product, identified by the product ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose images are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ProductImagesDto"/> for the specified product, or an empty collection if no images are found.</returns>
    Task<IEnumerable<ProductImagesDto>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Retrieves a collection of ProductImagesDto objects that are associated with the given ItemImage ID.
    /// </summary>
    /// <param name="itemImageId">The unique identifier of the item image.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of ProductImagesDto objects associated with the specified item image ID.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided item image ID is empty.</exception>
    Task<IEnumerable<ProductImagesDto>> GetByItemImageIdAsync(Guid itemImageId);

    /// <summary>
    /// Creates a new product image association based on the provided request data.
    /// </summary>
    /// <param name="request">The request data containing information about the product and image to associate.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created product image data as a <see cref="ProductImagesDto"/>. Returns null if the creation fails.</returns>
    Task<ProductImagesDto?> CreateAsync(ProductImagesCreateRequest request);

    /// <summary>
    /// Updates the details of an existing product image based on the specified update request.
    /// </summary>
    /// <param name="request">The request containing the details to update an existing product image.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// the updated product image as a <see cref="ProductImagesDto"/>, or null if the update fails.</returns>
    Task<ProductImagesDto?> UpdateAsync(ProductImagesUpdateRequest request);

    /// <summary>
    /// Deletes a product image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product image to delete.</param>
    /// <exception cref="ArgumentException">Thrown if the provided id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if no product image is found with the provided id.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}