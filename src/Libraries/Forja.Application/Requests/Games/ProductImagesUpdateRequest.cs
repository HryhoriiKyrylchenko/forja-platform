namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update product images in the system.
/// </summary>
public class ProductImagesUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product image update request.
    /// This property is required and represents the identifier associated with the specific product image update operation.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the product associated with the update request.
    /// This property is required to identify the specific product whose images are being updated.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the item image associated with the product.
    /// This property is required and must be a valid GUID.
    /// </summary>
    [Required]
    public Guid ItemImageId { get; set; }
}