namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a game mechanic, which includes
/// relevant information about the game mechanic.
/// </summary>
public class MechanicDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the mechanic.
    /// </summary>
    /// <remarks>
    /// This property represents a globally unique identifier (GUID) used to uniquely identify a mechanic entity.
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the mechanic.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the mechanic.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the logo associated with the mechanic.
    /// </summary>
    public string? LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the entity is marked as deleted.
    /// This property is used to identify if the mechanic is logically removed from active usage.
    /// </summary>
    public bool IsDeleted { get; set; }
}