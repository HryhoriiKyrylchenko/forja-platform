namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update a tag in the system.
/// </summary>
/// <remarks>
/// This class is used to encapsulate the necessary data for updating a tag.
/// It includes properties for the tag's identifier and title. Validation attributes
/// are applied to ensure the proper format and length of the data.
/// </remarks>
public class TagUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    /// <remarks>
    /// This property is required and is used to uniquely identify a tag being updated.
    /// It is represented as a GUID.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// The title is required and must not exceed 30 characters in length.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;
}