namespace Forja.Domain.Entities.UserProfile;

[Table("Users", Schema = "user-profile")]
public class User : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid KeycloakUserId { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;
    
    [MaxLength(30)]
    public string? Firstname { get; set; }
    
    [MaxLength(30)]
    public string? Lastname { get; set; }
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    public string? AvatarUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? ModifiedAt { get; set; }

    public virtual Cart? Cart { get; set; }
    
    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];
    
    public virtual ICollection<Review> Reviews { get; set; } = [];
    
    public virtual ICollection<Payment> Payments { get; set; } = [];
    
    public virtual ICollection<Order> Orders { get; set; } = [];
    
    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = [];
    
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = [];
    
    public virtual ICollection<TicketMessage> TicketMessages { get; set; } = [];
    
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
}