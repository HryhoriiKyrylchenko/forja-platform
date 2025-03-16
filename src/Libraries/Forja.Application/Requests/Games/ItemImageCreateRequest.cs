namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create an item image, including its URL and alternative text.
/// </summary>
public class ItemImageCreateRequest
{
    /// <summary>
    /// Gets or sets the URL of the image.
    /// </summary>
    /// <remarks>
    /// This property specifies the location of the image file that is associated with the item.
    /// The value should be a valid URL pointing to the image resource.
    /// By default, this property is initialized to an empty string.
    /// </remarks>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative text for the image.
    /// This property provides a textual description of the image, commonly used for accessibility purposes or
    /// as a fallback when the image cannot be displayed.
    /// </summary>
    public string ImageAlt { get; set; } = string.Empty;
}