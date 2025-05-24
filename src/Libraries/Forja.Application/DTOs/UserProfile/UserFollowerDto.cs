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
    public string FollowerUsername { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the avatar for the follower in the UserFollower entry.
    /// </summary>
    public string? FollowerAvatarUrl { get; set; }

    /// <summary>
    /// Represents the unique tag associated with the follower, used for display or identification purposes.
    /// </summary>
    public string? FollowerTag { get; set; } = string.Empty;

    /// <summary>
    /// The unique ID of the user who is being followed.
    /// </summary>
    public Guid FollowedId { get; set; }

    /// <summary>
    /// The name of the user who is being followed.
    /// </summary>
    public string FollowedUsername { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the avatar associated with the followed user.
    /// </summary>
    public string? FollowedAvatarUrl { get; set; }

    /// <summary>
    /// Represents the unique tag associated with the followed user.
    /// </summary>
    public string? FollowedTag { get; set; } = string.Empty;
}
