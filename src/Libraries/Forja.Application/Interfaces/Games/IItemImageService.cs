namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides an abstraction for operations related to managing item images.
/// </summary>
public interface IItemImageService
{
    /// <summary>
    /// Asynchronously retrieves all item images.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a collection of <c>ItemImageDto</c> that represent the item images.</returns>
    Task<IEnumerable<ItemImageDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves an ItemImageDto entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item image to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved ItemImageDto if found; otherwise, null.</returns>
    Task<ItemImageDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all item images along with their associated product images.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable of
    /// <see cref="ItemImageDto"/> objects, each representing an item image with its associated product images.
    /// </returns>
    Task<IEnumerable<ItemImageDto>> GetAllWithProductImagesAsync();

    /// <summary>
    /// Creates a new item image based on the given request.
    /// </summary>
    /// <param name="request">
    /// The request object containing the details necessary to create the item image,
    /// including the image URL and alternative text.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. Upon completion, yields an
    /// <see cref="ItemImageDto"/> containing the details of the created item image, or null if creation fails.
    /// </returns>
    Task<ItemImageDto?> CreateAsync(ItemImageCreateRequest request);

    /// <summary>
    /// Updates an existing item image with the provided details.
    /// </summary>
    /// <param name="request">An object containing the updated details of the item image, including its ID, new image URL, and alternative text.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated item image as a <see cref="ItemImageDto"/>, or null if the update could not be completed.</returns>
    Task<ItemImageDto?> UpdateAsync(ItemImageUpdateRequest request);

    /// <summary>
    /// Deletes an item image with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item image to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided identifier is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the item image with the given identifier is not found.</exception>
    Task DeleteAsync(Guid id);
}