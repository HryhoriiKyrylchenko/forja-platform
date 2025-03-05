namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// Data transfer object for UserFollower entity.
/// Simplifies the data returned for UserFollower-related operations.
/// </summary>
public class UserFollowerDto
{
    /// <summary>
    /// Represents the unique ID for the UserFollower entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique ID of the user who is following another user.
    /// </summary>
    public Guid FollowerId { get; set; }

    /// <summary>
    /// The name of the user who is following.
    /// </summary>
    public string FollowerName { get; set; } = string.Empty;

    /// <summary>
    /// The unique ID of the user who is being followed.
    /// </summary>
    public Guid FollowedId { get; set; }

    /// <summary>
    /// The name of the user who is being followed.
    /// </summary>
    public string FollowedName { get; set; } = string.Empty;
}
