namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for holding information about an item's image.
/// </summary>
public class ItemImageDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the item image.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the item's image.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative text for the image, providing descriptive information
    /// that can be used in cases where the image cannot be loaded or for accessibility purposes.
    /// </summary>
    public string ImageAlt { get; set; } = string.Empty;
}