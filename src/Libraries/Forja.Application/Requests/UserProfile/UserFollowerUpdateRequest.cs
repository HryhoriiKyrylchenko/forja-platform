namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Request model for updating a UserFollower entry.
/// </summary>
public class UserFollowerUpdateRequest
{
    /// <summary>
    /// The unique identifier of the user follower update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
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