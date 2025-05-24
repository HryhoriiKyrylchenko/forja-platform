namespace Forja.Domain.Entities.Support;

/// <summary>
/// Represents a support ticket in the system.
/// </summary>
[Table("SupportTickets", Schema = "support")]
public class SupportTicket : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the support ticket.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the support ticket.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the subject of the support ticket.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the support ticket.
    /// </summary>
    [MaxLength(2000)]
    public string Description { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the status of the support ticket.
    /// </summary>
    [Required]
    public SupportTicketStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the support ticket was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the date and time when the support ticket was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who created the support ticket.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of messages associated with the support ticket.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<TicketMessage> Messages { get; set; } = [];
}