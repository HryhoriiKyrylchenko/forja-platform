namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to associate a product with mature content.
/// </summary>
public class ProductMatureContentCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    /// <remarks>
    /// This property is used to specify the product associated with the request. It is required and must be a valid <see cref="Guid"/>.
    /// </remarks>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the mature content associated with the product.
    /// </summary>
    /// <remarks>
    /// This property represents a unique identifier (GUID) for the mature content,
    /// which is required to indicate specific content falling under mature categories.
    /// </remarks>
    [Required]
    public Guid MatureContentId { get; set; }
}