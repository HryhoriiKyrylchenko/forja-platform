namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update the properties related to mature content within a game.
/// </summary>
/// <remarks>
/// This class encapsulates the data required to update information about mature content,
/// including identifiers, names, descriptions, and associated media URLs.
/// </remarks>
public class MatureContentUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the mature content update request.
    /// </summary>
    /// <remarks>
    /// This identifier is required and must represent a valid GUID.
    /// It serves as a primary key to distinguish and reference the request item.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name associated with the mature content update request.
    /// This value is required and its length cannot exceed 50 characters.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the description of the mature content. This property provides additional details or context for the related content.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the logo associated with the request.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;
}