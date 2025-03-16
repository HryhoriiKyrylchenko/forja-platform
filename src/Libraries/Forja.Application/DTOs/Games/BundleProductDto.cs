namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a product included in a bundle.
/// </summary>
public class BundleProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle product.
    /// </summary>
    /// <remarks>
    /// This property uniquely identifies each bundle product within the system using a GUID.
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the bundle associated with a product.
    /// </summary>
    public Guid BundleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated product.
    /// </summary>
    public Guid ProductId { get; set; }
}