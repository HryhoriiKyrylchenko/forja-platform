namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing item image entities in the repository.
/// </summary>
public interface IItemImageRepository
{
    /// <summary>
    /// Gets all item images.
    /// </summary>
    /// <returns>A collection of all item images.</returns>
    Task<IEnumerable<ItemImage>> GetAllAsync();

    /// <summary>
    /// Gets an item image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item image.</param>
    /// <returns>The item image with the specified ID, or null if not found.</returns>
    Task<ItemImage?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new item image to the repository.
    /// </summary>
    /// <param name="itemImage">The item image to add.</param>
    /// <returns>The added item image.</returns>
    Task<ItemImage?> AddAsync(ItemImage itemImage);

    /// <summary>
    /// Updates an existing item image in the repository.
    /// </summary>
    /// <param name="itemImage">The item image with updated information.</param>
    /// <returns>The updated item image.</returns>
    Task<ItemImage?> UpdateAsync(ItemImage itemImage);

    /// <summary>
    /// Deletes an item image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item image to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all item images with their associated product images.
    /// </summary>
    /// <returns>A collection of item images including their related product images.</returns>
    Task<IEnumerable<ItemImage>> GetAllWithProductImagesAsync();
}