namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to update a review for a product.
/// </summary>
public class ReviewUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the review update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the review update request.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the product associated with the review update request.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the review reflects a positive rating.
    /// </summary>
    [Required]
    public bool PositiveRating { get; set; }

    /// <summary>
    /// Gets or sets the comment associated with the review update request.
    /// This property is used to provide a textual description or feedback
    /// about the product in the user's review.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}