namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a game bundle.
/// </summary>
public class BundleDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the bundle.
    /// Represents the name or label used to identify the bundle.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the bundle.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total price of the bundle.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the bundle was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Indicates whether the bundle is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the list of products included in the bundle.
    /// </summary>
    public List<ProductDto> BundleProducts { get; set; } = [];
}