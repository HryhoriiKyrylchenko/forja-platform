namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for product genres, containing
/// identification details for the product, the genre, and their relationship.
/// </summary>
public class ProductGenresDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the product-genre relationship.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the genre associated with the product.
    /// </summary>
    public Guid GenreId { get; set; }
}