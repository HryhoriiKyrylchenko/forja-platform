namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request model for updating a mechanic entity in the application.
/// </summary>
public class MechanicUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the mechanic. This property is required.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the mechanic to be updated.
    /// This property is required and has a maximum length of 50 characters.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the mechanic update request.
    /// Represents detailed information or explanation about the mechanic.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the mechanic is marked as deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}