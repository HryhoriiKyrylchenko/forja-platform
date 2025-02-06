namespace Forja.Domain.Entities.Support;

[Table("TicketMessages", Schema = "support")]
public class TicketMessage : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("SupportTicket")]
    public Guid SupportTicketId { get; set; }
    
    [ForeignKey("Sender")]
    public Guid SenderId { get; set; }

    public bool IsSupportAgent { get; set; } = false;

    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = String.Empty;

    public DateTime SentAt { get; set; } = DateTime.Now;
    
    public virtual SupportTicket SupportTicket { get; set; } = null!;
    
    public virtual User Sender { get; set; } = null!;
}