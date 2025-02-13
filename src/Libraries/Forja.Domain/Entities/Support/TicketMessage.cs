namespace Forja.Domain.Entities.Support;

/// <summary>
/// Represents a message in a support ticket.
/// </summary>
[Table("TicketMessages", Schema = "support")]
public class TicketMessage : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the ticket message.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the related support ticket.
    /// </summary>
    [ForeignKey("SupportTicket")]
    public Guid SupportTicketId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier of the sender.
    /// </summary>
    [ForeignKey("Sender")]
    public Guid SenderId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the sender is a support agent.
    /// </summary>
    public bool IsSupportAgent { get; set; } = false;

    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the date and time when the message was sent.
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Gets or sets the related support ticket.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual SupportTicket SupportTicket { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the sender of the message.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User Sender { get; set; } = null!;
}