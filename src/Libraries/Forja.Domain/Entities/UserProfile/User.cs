using System.Runtime.CompilerServices;

namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents a user in the system.
/// </summary>
[Table("Users", Schema = "user-profile")]
public class User : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Keycloak user identifier.
    /// </summary>
    [Required]
    public string KeycloakUserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    [MaxLength(30)]
    public string? Firstname { get; set; }
    
    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    [MaxLength(30)]
    public string? Lastname { get; set; }
    
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the URL of the user's avatar.
    /// </summary>
    public string? AvatarUrl { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the date and time when the user was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the cart associated with the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Cart? Cart { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of games in the user's library.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of reviews written by the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Review> Reviews { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of payments made by the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of orders placed by the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of achievements earned by the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of support tickets created by the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of messages in the user's support tickets.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<TicketMessage> TicketMessages { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of audit logs related to the user.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
}