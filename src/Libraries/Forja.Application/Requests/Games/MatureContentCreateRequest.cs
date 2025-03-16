namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a new mature content entity.
/// </summary>
public class MatureContentCreateRequest
{
    /// <summary>
    /// Gets or sets the name associated with the mature content create request.
    /// This property is required and has a maximum length of 50 characters.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description for the mature content.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the logo associated with the mature content.
    /// </summary>
    /// <remarks>
    /// This property is optional. If provided, it should contain a valid URL pointing to the logo image.
    /// </remarks>
    public string? LogoUrl { get; set; } = string.Empty;
}