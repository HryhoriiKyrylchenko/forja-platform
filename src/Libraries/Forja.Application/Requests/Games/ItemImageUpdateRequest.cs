namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update the image information of an item.
/// </summary>
public class ItemImageUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the item image update request.
    /// </summary>
    /// <remarks>
    /// This property is required and represents the primary identifier
    /// for the item whose image is being updated.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the image associated with the item.
    /// </summary>
    /// <remarks>
    /// The ImageUrl property is used to define the location of the item's image.
    /// It should typically be a valid URL of the image resource.
    /// </remarks>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative text for the image associated with the item.
    /// This property is used for accessibility purposes and provides descriptive
    /// text for the image if it cannot be rendered or accessed.
    /// </summary>
    public string ImageAlt { get; set; } = string.Empty;
}