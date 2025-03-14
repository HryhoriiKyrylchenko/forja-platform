namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update the genres associated with a product.
/// This class is used to specify the necessary data required to update a product's genre relationship.
/// </summary>
public class ProductGenresUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product genre update request.
    /// This property is required for identifying the specific update operation.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product associated with the genres update request.
    /// </summary>
    /// <remarks>
    /// This property is required and represents a unique identifier (GUID) for the product being updated.
    /// </remarks>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the genre associated with the product.
    /// </summary>
    /// <remarks>
    /// This property represents the foreign key relationship between a product and its corresponding genre.
    /// It is required for updating the genre information of a product.
    /// </remarks>
    [Required]
    public Guid GenreId { get; set; }
}