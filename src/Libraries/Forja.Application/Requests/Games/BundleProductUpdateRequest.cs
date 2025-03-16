namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update a bundle-product relationship.
/// </summary>
public class BundleProductUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle product update request.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier for the bundle associated with the product update request.
    /// </summary>
    public Guid BundleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product associated with the bundle.
    /// </summary>
    public Guid ProductId { get; set; }
}