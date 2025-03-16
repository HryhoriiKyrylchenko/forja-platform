namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a new tag in the system.
/// </summary>
public class TagCreateRequest
{
    /// <summary>
    /// Gets or sets the title of the tag.
    /// The title is a required property with a maximum length of 30 characters.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;
}