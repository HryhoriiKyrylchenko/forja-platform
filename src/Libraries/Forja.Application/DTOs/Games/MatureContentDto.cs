namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents the Data Transfer Object (DTO) for mature content associated with a game.
/// </summary>
/// <remarks>
/// This DTO contains information about the mature content such as its identifier, name, description,
/// and an optional logo URL related to the content.
/// </remarks>
public class MatureContentDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the mature content.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name associated with the mature content.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the mature content.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the logo associated with the mature content.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;
}