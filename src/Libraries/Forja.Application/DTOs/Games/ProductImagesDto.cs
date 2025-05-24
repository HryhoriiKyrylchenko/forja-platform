namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for product images with IDs associated to specific products and their images.
/// </summary>
public class ProductImagesDto
{
    /// <summary>
    /// Represents the unique identifier for a product image.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product associated with the image.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the image associated with a product item.
    /// </summary>
    public Guid ItemImageId { get; set; }
}