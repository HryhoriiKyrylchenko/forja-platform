namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// A Data Transfer Object (DTO) representing a review for a product by a user.
/// </summary>
public class ReviewDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the review.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the review.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product associated with the review.
    /// </summary>
    /// <remarks>
    /// This property represents the foreign key linking a review to a specific product.
    /// It is used to associate and retrieve reviews for a particular product within the system.
    /// </remarks>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Indicates whether the review contains a positive rating for the associated product.
    /// </summary>
    /// <remarks>
    /// This property is used to differentiate between positive and negative feedback from users.
    /// A value of <c>true</c> represents a positive rating, while <c>false</c> represents a negative rating.
    /// </remarks>
    public bool PositiveRating { get; set; }

    /// <summary>
    /// Represents the textual content of a review provided by a user.
    /// </summary>
    /// <remarks>
    /// The content of the comment is user-generated and reflects their thoughts
    /// and opinions about the associated product. It is stored as a non-nullable
    /// string and initialized with an empty value.
    /// </remarks>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the review was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}