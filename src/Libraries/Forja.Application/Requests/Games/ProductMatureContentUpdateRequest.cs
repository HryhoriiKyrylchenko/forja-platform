namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update mature content information for a specific product.
/// </summary>
public class ProductMatureContentUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the specific request.
    /// This property is required and is used to uniquely identify the instance of the ProductMatureContentUpdateRequest.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product to be updated.
    /// This property is required and is used to associate the update request with a specific product.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the mature content associated with the product.
    /// </summary>
    /// <remarks>
    /// This property is used to specify the unique identifier of the mature content
    /// that is being linked to or updated for a specific product.
    /// It is a required field and must be provided during a request.
    /// </remarks>
    [Required]
    public Guid MatureContentId { get; set; }
}