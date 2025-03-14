namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a mechanic in the system.
/// Contains necessary details for creating a new game mechanic.
/// </summary>
public class MechanicCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the mechanic being created.
    /// This property is required and must not exceed 50 characters in length.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Provides a detailed explanation or representation of the mechanic being created.
    /// This property enables a descriptive text that can be used to better understand the context or purpose of the mechanic.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the logo associated with the mechanic.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;
}