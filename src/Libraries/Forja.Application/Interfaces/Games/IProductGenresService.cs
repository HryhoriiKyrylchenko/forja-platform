namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides methods for managing associations between products and genres within the system.
/// </summary>
public interface IProductGenresService
{
    /// <summary>
    /// Retrieves all product genres asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a collection of <see cref="ProductGenresDto"/> representing all product genres.
    /// </returns>
    Task<IEnumerable<ProductGenresDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a product genre by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product genre to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ProductGenresDto"/> if found; otherwise, null.</returns>
    Task<ProductGenresDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a collection of ProductGenresDto associated with the specified product identifier.
    /// Throws an exception if no genres are found for the provided product ID.
    /// </summary>
    /// <param name="productId">The identifier of the product for which the associated genres are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of ProductGenresDto objects.</returns>
    Task<IEnumerable<ProductGenresDto>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Retrieves a collection of product genres associated with the specified genre ID.
    /// </summary>
    /// <param name="genreId">The unique identifier of the genre for which the product genres are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of
    /// <see cref="ProductGenresDto"/> instances associated with the specified genre ID.
    /// </returns>
    Task<IEnumerable<ProductGenresDto>> GetByGenreIdAsync(Guid genreId);

    /// <summary>
    /// Creates a new relationship between a product and a genre based on the specified request data.
    /// </summary>
    /// <param name="request">The data required to create a new product-genre relationship.</param>
    /// <returns>A <see cref="ProductGenresDto"/> representing the created product-genre record, or null if creation failed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the creation of the product-genre relationship fails.</exception>
    Task<ProductGenresDto?> CreateAsync(ProductGenresCreateRequest request);

    /// <summary>
    /// Updates an existing product-genre relationship with the given data.
    /// </summary>
    /// <param name="request">The request containing the details to update an existing product-genre relationship, including its ID, ProductId, and GenreId.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated product-genre DTO or null if the update fails.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided request is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the product-genre relationship with the specified ID is not found.</exception>
    Task<ProductGenresDto?> UpdateAsync(ProductGenresUpdateRequest request);

    /// <summary>
    /// Deletes a product-genre association by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-genre association to delete.</param>
    /// <exception cref="ArgumentException">Thrown when the provided id is empty or invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no product-genre association is found with the specified id.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}