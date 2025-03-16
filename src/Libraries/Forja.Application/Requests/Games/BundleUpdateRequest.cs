namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update an existing game bundle.
/// </summary>
/// <remarks>
/// This class contains the necessary information for updating a bundle,
/// including its unique identifier, title, description, price, and status.
/// </remarks>
public class BundleUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the title of the bundle being updated. This property is required and cannot be null or empty.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the bundle.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the total price of the bundle in the update request.
    /// This property defines the aggregate cost associated with the bundle
    /// and is expressed as a decimal value.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Indicates whether the bundle is currently active.
    /// </summary>
    public bool IsActive { get; set; }
}