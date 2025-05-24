namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request for creating a review for a product by a user.
/// </summary>
public class ReviewCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user creating the review.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the product being reviewed.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Indicates whether the review has a positive rating or not.
    /// </summary>
    [Required]
    public bool PositiveRating { get; set; }

    /// <summary>
    /// Gets or sets the comment provided in the review.
    /// This can include the user's detailed feedback or remarks about the product.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp indicating when the review was created.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }
}