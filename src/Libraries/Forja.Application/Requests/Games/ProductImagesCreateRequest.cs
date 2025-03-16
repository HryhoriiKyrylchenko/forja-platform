namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create an association between a product and an image.
/// </summary>
public class ProductImagesCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    /// <remarks>
    /// This property is required and serves as a reference to the specific product
    /// associated with the images being created or managed.
    /// </remarks>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the item image associated with a product.
    /// </summary>
    /// <remarks>
    /// This property represents the ID of the specific item image that is linked to a product.
    /// It is a required field and must be provided when creating a product image entry.
    /// </remarks>
    [Required]
    public Guid ItemImageId { get; set; }
}