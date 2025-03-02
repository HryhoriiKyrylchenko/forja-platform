namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents the relationship between users in terms of following and being followed.
/// </summary>
[Table("UserFollowers", Schema = "user-profile")]
public class UserFollower
{
    /// <summary>
    /// Represents the unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier for the follower user in the user-follower relationship.
    /// </summary>
    /// <remarks>
    /// This property is a foreign key linking to the Follower entity. It is utilized to identify
    /// the user who is following another user within the user-profile schema.
    /// </remarks>
    [ForeignKey("Follower")]
    public Guid FollowerId { get; set; }

    /// <summary>
    /// Represents the unique identifier of the user being followed in the system.
    /// </summary>
    [ForeignKey("Followed")]
    public Guid FollowedId { get; set; }

    /// <summary>
    /// Gets or sets the user who is the follower in the relationship.
    /// </summary>
    /// <remarks>
    /// This property represents the user entity who follows another user.
    /// It establishes a relationship between the current entity and
    /// the <c>User</c> entity, indicating a unidirectional following connection.
    /// </remarks>
    public virtual User Follower { get; set; } = null!;

    /// <summary>
    /// Represents the user that is being followed in the relationship.
    /// </summary>
    public virtual User Followed { get; set; } = null!;
}