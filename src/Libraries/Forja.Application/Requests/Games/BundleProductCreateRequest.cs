namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a bundle product association.
/// </summary>
public class BundleProductCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle.
    /// </summary>
    [Required]
    public Guid BundleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the product associated with this bundle.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
}