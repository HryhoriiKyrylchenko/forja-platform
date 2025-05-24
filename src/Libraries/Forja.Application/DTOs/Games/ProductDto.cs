namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a product.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// Represents the main name or heading used to identify the product.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the product.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the product.
    /// </summary>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the product.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Indicates whether the product is active or not.
    /// It determines if the product is available or enabled in the system.
    /// </summary>
    public bool IsActive { get; set; }
}