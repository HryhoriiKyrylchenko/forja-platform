namespace Forja.API.DTOs.UserProfile;

/// <summary>
/// Request model for creating a UserFollower entry.
/// </summary>
public class FollowerCreateRequest
{
    /// <summary>
    /// The unique identifier of the user who is the follower.
    /// </summary>
    [Required]
    public Guid FollowerId { get; set; }

    /// <summary>
    /// The unique identifier of the user who is being followed.
    /// </summary>
    [Required]
    public Guid FollowedId { get; set; }
}
