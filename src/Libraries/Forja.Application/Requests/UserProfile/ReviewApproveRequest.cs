namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to approve or reject a review.
/// </summary>
public class ReviewApproveRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the review approval request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Indicates whether the request has been approved or not.
    /// </summary>
    [Required]
    public bool IsApproved { get; set; }
}