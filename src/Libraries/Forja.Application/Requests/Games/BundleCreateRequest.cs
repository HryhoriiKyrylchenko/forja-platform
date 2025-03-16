namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a bundle.
/// </summary>
public class BundleCreateRequest
{
    /// <summary>
    /// Gets or sets the title of the bundle.
    /// This property is required and must contain a non-empty value.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the bundle.
    /// This property provides a detailed overview or additional information about the bundle being created.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total price of the bundle.
    /// This represents the cumulative cost associated with the bundle.
    /// </summary>
    public decimal TotalPrice { get; set; }
}