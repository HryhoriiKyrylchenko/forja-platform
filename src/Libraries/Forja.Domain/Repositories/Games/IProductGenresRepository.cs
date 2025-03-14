namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing the association between products and genres.
/// </summary>
public interface IProductGenresRepository
{
    /// <summary>
    /// Gets all product-genre relationships.
    /// </summary>
    /// <returns>A collection of all product-genre relationships.</returns>
    Task<IEnumerable<ProductGenres>> GetAllAsync();

    /// <summary>
    /// Gets a specific product-genre relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-genre relationship.</param>
    /// <returns>The product-genre relationship with the specified ID, or null if not found.</returns>
    Task<ProductGenres?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new product-genre relationship to the repository.
    /// </summary>
    /// <param name="productGenre">The product-genre relationship to add.</param>
    /// <returns>The added product-genre relationship.</returns>
    Task<ProductGenres?> AddAsync(ProductGenres productGenre);

    /// <summary>
    /// Updates an existing product-genre relationship.
    /// </summary>
    /// <param name="productGenre">The product-genre entity containing updated information.</param>
    /// <returns>The updated product-genre entity, or null if the update was unsuccessful.</returns>
    Task<ProductGenres?> UpdateAsync(ProductGenres productGenre);

    /// <summary>
    /// Deletes a product-genre relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product-genre relationship to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all product-genre relationships including the associated product and genre details.
    /// </summary>
    /// <returns>A collection of product-genre relationships with their products and genres.</returns>
    Task<IEnumerable<ProductGenres>> GetAllWithDetailsAsync();

    /// <summary>
    /// Finds product-genre relationships by product ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product-genre relationships associated with the product ID.</returns>
    Task<IEnumerable<ProductGenres>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Finds product-genre relationships by genre ID.
    /// </summary>
    /// <param name="genreId">The unique identifier of the genre.</param>
    /// <returns>A collection of product-genre relationships associated with the genre ID.</returns>
    Task<IEnumerable<ProductGenres>> GetByGenreIdAsync(Guid genreId);
}