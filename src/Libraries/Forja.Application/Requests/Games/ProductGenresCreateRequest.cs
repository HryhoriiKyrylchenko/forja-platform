namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a relationship between a product and a genre.
/// </summary>
public class ProductGenresCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// This property is required and is used to associate a product with a given genre.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the genre associated with the product.
    /// </summary>
    [Required]
    public Guid GenreId { get; set; }
}